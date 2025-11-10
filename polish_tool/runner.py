# -*- coding: utf-8 -*-
"""
核心处理引擎模块。

功能:
- 包含 process_file 函数，负责完整处理单个 .mtf 文件。
- 管理并发、任务分发（单行/批量）、失败回退、进度更新和结果写回。
"""
import os
import time
from concurrent.futures import ThreadPoolExecutor, as_completed
from typing import List, Tuple, Dict, Any, Optional

# 导入重构后的其他模块
from . import file_io
from . import prompts
from . import api_client
from . import text_processing
from . import glossary_tool
from . import utils

# 尝试导入 requests，主要用于创建 session
try:
    import requests
except ImportError:
    requests = None

def _accumulate_usage(usage_acc: Dict[str, float], usage: Dict[str, Any]):
    """安全地累加 API token 用量，忽略非数字类型的值。"""
    if not isinstance(usage, dict):
        return
    for k, v in usage.items():
        if isinstance(v, (int, float)):
            usage_acc[k] = usage_acc.get(k, 0.0) + v

def process_file(
    root_dir: str,
    file_path: str,
    api_key: str,
    cfg: Dict[str, Any],
    progress: Dict[str, Any],
    usage_acc: Dict[str, float]
) -> Tuple[int, int]:
    """处理单个文件的完整流程引擎。"""
    file_name = os.path.basename(file_path)
    
    # 步骤 1: 读取与准备任务
    try:
        pairs = file_io.read_pairs(file_path)
    except Exception as e:
        print(f"[错误] 读取文件失败: {file_path}, 原因: {e}")
        return 0, 0

    jobs = [(idx, lhs, rhs) for idx, (line, lhs, rhs) in enumerate(pairs) if lhs and rhs]
    if not jobs:
        return 0, 0

    # 步骤 2: 加载配置
    system_prompt = prompts.build_system_prompt(root_dir, cfg['bg_mode'], file_name)
    canon_rules = text_processing.load_canonical_rules(root_dir)
    glossary_items = glossary_tool.load_glossary_inputs(progress.get('glossary_in_dirs', []))

    # 步骤 3: 术语挖掘 (如果启用)
    if progress.get('mine_terms'):
        terms_map = progress.setdefault('terms_map', {})
        glossary_tool.mine_terms_from_jobs(jobs, file_name, terms_map)

    ok_count = fail_count = 0
    session = requests.Session() if requests else None
    
    with ThreadPoolExecutor(max_workers=cfg['max_workers']) as executor:
        # 步骤 4: 提交并发任务
        future_map = {}
        if cfg['mode'] == 'batch':
            for i in range(0, len(jobs), cfg['group_size']):
                batch = jobs[i:i + cfg['group_size']]
                batch_glossary = []
                if glossary_items:
                    seen = set()
                    for _, lhs, _ in batch:
                        for match in glossary_tool.find_glossary_matches(lhs, glossary_items):
                            key = tuple(match.values())
                            if key not in seen:
                                seen.add(key)
                                batch_glossary.append(match)
                
                user_prompt = prompts.build_batch_user_prompt(file_name, batch, batch_glossary)
                future = executor.submit(
                    api_client.call_deepseek_batch, api_key, system_prompt, user_prompt,
                    cfg['timeout'], cfg['temperature'], cfg['max_tokens'] * 2, session
                )
                future_map[future] = ('batch', batch, user_prompt)
        else: # mode == 'line'
            for idx, lhs, rhs in jobs:
                matches = glossary_tool.find_glossary_matches(lhs, glossary_items)
                user_prompt = prompts.build_user_prompt(file_name, lhs, rhs, matches)
                future = executor.submit(
                    api_client.call_deepseek_line, api_key, system_prompt, user_prompt,
                    cfg['timeout'], cfg['temperature'], cfg['max_tokens'], session
                )
                future_map[future] = ('line', [(idx, lhs, rhs)], user_prompt)

        # 步骤 5 & 6: 异步收集结果并处理失败回退
        for future in as_completed(future_map):
            kind, payload, user_prompt = future_map[future]
            results_map: Dict[int, str] = {}
            raw_res, usage = "", {}

            try:
                # 尝试获取 API 结果
                if kind == 'batch':
                    results_map, raw_res, usage = future.result()
                else: # line
                    rhs_res, raw_res, usage = future.result()
                    results_map = {payload[0][0]: rhs_res}
                
                # 安全地累加用量
                _accumulate_usage(usage_acc, usage)

            except Exception as e:
                # API 调用失败或用量累加失败
                print(f"[警告] 处理 API 响应失败 (文件: {file_name}, 模式: {kind}): {e}")
                # 如果是批量模式失败，则降级为单行模式重试
                if kind == 'batch':
                    print(f"[回退] 对 {file_name} 的失败批次，转为单行模式重试...")
                    for idx, lhs, rhs in payload:
                        try:
                            retry_matches = glossary_tool.find_glossary_matches(lhs, glossary_items)
                            retry_prompt = prompts.build_user_prompt(file_name, lhs, rhs, retry_matches)
                            rhs_res, raw_res, usage = api_client.call_deepseek_line(
                                api_key, system_prompt, retry_prompt,
                                cfg['timeout'], cfg['temperature'], cfg['max_tokens'], session
                            )
                            results_map[idx] = rhs_res
                            _accumulate_usage(usage_acc, usage) # 安全地累加用量
                        except Exception as e2:
                            fail_count += 1; progress['fail'] += 1
                            reason = f'批量回退失败: {e2}'
                            progress['fail_list'].append({'file': file_name, 'index': idx, 'lhs': lhs, 'reason': reason})
                            if cfg['log_dir']: utils.write_line_log(cfg['log_dir'], file_name, idx, lhs, rhs, system_prompt, retry_prompt, None, None, None, reason)
                else: # 单行模式也失败了
                    idx, lhs, rhs = payload[0]
                    fail_count += 1; progress['fail'] += 1
                    reason = f'API 调用失败: {e}'
                    progress['fail_list'].append({'file': file_name, 'index': idx, 'lhs': lhs, 'reason': reason})
                    if cfg['log_dir']: utils.write_line_log(cfg['log_dir'], file_name, idx, lhs, rhs, system_prompt, user_prompt, None, None, usage, reason)
                continue # 处理下一个已完成的 future

            # 步骤 7: 校验与更新内存
            for idx, lhs, old_rhs in payload:
                if idx not in results_map: continue

                new_rhs_raw = results_map[idx]
                new_rhs = text_processing.sanitize_rhs(new_rhs_raw)
                reason = ""
                
                if not new_rhs:
                    reason = '模型返回格式不合规'
                elif text_processing.get_placeholders(lhs) - text_processing.get_placeholders(new_rhs):
                    reason = '占位符不匹配'
                
                if reason:
                    fail_count += 1; progress['fail'] += 1
                    progress['fail_list'].append({'file': file_name, 'index': idx, 'lhs': lhs, 'reason': reason})
                    if cfg['log_dir']: utils.write_line_log(cfg['log_dir'], file_name, idx, lhs, old_rhs, system_prompt, user_prompt, raw_res, new_rhs, usage, reason)
                else:
                    # 成功！应用规范化规则并更新内存中的 pairs
                    final_rhs = text_processing.canonicalize_rhs(new_rhs, canon_rules)
                    pairs[idx] = (pairs[idx][0], lhs, final_rhs)
                    ok_count += 1
                    progress['ok'] += 1
                    if cfg['log_dir']: utils.write_line_log(cfg['log_dir'], file_name, idx, lhs, old_rhs, system_prompt, user_prompt, raw_res, final_rhs, usage, None)

            # 步骤 10: 更新进度
            progress['done'] += len(payload)
            now = time.time()
            if now - progress['last_update_time'] > cfg['progress_interval']:
                progress['last_update_time'] = now
                elapsed = now - progress['start_time']
                rate = progress['done'] / elapsed if elapsed > 0 else 0
                eta = (progress['total_lines'] - progress['done']) / rate if rate > 0 else float('inf')
                print(f"[进度] 行 {int(progress['done'])}/{int(progress['total_lines'])}, "
                      f"文件 {int(progress['files_done'])}/{int(progress['files_total'])}, "
                      f"成功 {int(progress['ok'])}, 失败 {int(progress['fail'])}, "
                      f"速率 {rate:.2f} 行/秒, ETA {utils.fmt_eta(eta)}")

    # 步骤 9: 文件级写回
    try:
        file_io.write_pairs(file_path, pairs)
        print(f"[写入] {file_path} 完成：成功 {ok_count}，失败 {fail_count}")
    except Exception as e:
        print(f"[错误] 写回文件失败: {file_path}, 原因: {e}")

    progress['files_done'] += 1
    return ok_count, fail_count

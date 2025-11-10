# -*- coding: utf-8 -*-
"""
主入口脚本，用于批量并行润色 .mtf 文件中的右侧译文。

该脚本作为总协调器，负责：
1. 解析命令行参数。
2. 查找并准备待处理的文件列表。
3. 初始化并管理全局状态（如进度、用量统计、日志目录）。
4. 调用 polish_tool.runner 中的核心处理引擎来执行任务。
5. 在任务结束后，打印总结报告、失败摘要，并写出挖掘的术语表。

用法:
  # 从项目根目录运行
  python -m polish_tool.main --letters A_ --mode batch
"""
import os
import sys
import time
import argparse
from typing import List, Dict, Any

# --- 路径设置 ---
# 将项目根目录（polish_tool 的父目录）添加到 sys.path，以确保模块能被正确导入
current_dir = os.path.dirname(os.path.abspath(__file__))
PROJECT_ROOT = os.path.abspath(os.path.join(current_dir, '..'))
if PROJECT_ROOT not in sys.path:
    sys.path.insert(0, PROJECT_ROOT)

# --- 模块导入 ---
# 从重构的 polish_tool 包中导入所需模块
from polish_tool import runner, file_io, glossary_tool, prompts

def main():
    """主函数：解析参数，协调整个润色流程。"""
    parser = argparse.ArgumentParser(
        description='批量并行润色 .mtf 右侧译文（DeepSeek）',
        formatter_class=argparse.RawTextHelpFormatter,
        epilog='示例用法:\n'
                 '  # 处理单个文件, 开启术语挖掘\n'
                 '  python -m polish_tool.main --only-files Araki_AfterGhostVisit.eng.mtf --mine-terms\n\n'
                 '  # 处理所有以 A 或 _ 开头的文件, 使用高性价比的批量模式\n'
                 '  python -m polish_tool.main --letters A_ --mode batch --group-size 16 --max-workers 32 --bg-mode auto\n'
    )
    parser.add_argument('--src-dir', default='English', help='源目录（包含 .mtf），相对于项目根目录')
    parser.add_argument('--only-files', default='', help='仅处理这些文件（逗号分隔），如 "A.mtf,B.mtf"')
    parser.add_argument('--letters', default='', help='按文件名首字符筛选，如 "A_"（下划线与 A）')
    parser.add_argument('--max-workers', type=int, default=16, help='并行请求数')
    parser.add_argument('--timeout', type=int, default=90, help='HTTP 超时（秒）')
    parser.add_argument('--temperature', type=float, default=1.0, help='采样温度，推荐 1.0')
    parser.add_argument('--max-tokens', type=int, default=400, help='单行回复最大 tokens')
    parser.add_argument('--mode', choices=['line', 'batch'], default='batch', help='请求模式')
    parser.add_argument('--group-size', type=int, default=12, help='batch 模式下每批的行数')
    parser.add_argument('--bg-mode', choices=['full', 'lite', 'auto'], default='auto', help='背景发送模式')
    parser.add_argument('--dry-run', action='store_true', help='仅统计文件，不执行 API 调用')
    parser.add_argument('--log-dir', default='logs/deepseek', help='行级日志输出目录')
    parser.add_argument('--progress-interval', type=float, default=5.0, help='进度更新心跳间隔（秒）')
    parser.add_argument('--glossary-in', default='glossary/in', help='术语表输入目录')
    parser.add_argument('--glossary-out', default='glossary/out', help='术语表输出目录')
    parser.add_argument('--mine-terms', action='store_true', help='启用独立术语挖掘功能')
    args = parser.parse_args()

    # 检查 API Key
    api_key = os.getenv('DEEPSEEK_API_KEY', '').strip()
    if not args.dry_run and not api_key:
        print('[错误] 缺少 API 密钥，请设置环境变量 DEEPSEEK_API_KEY')
        return

    # --- 1. 准备工作 ---
    src_path = os.path.join(PROJECT_ROOT, args.src_dir)

    # 查找目标文件
    targets = []
    if args.only_files:
        targets = [name.strip() for name in args.only_files.split(',') if name.strip()]
    elif os.path.isdir(src_path):
        letters = set((args.letters or '').lower())
        for name in sorted(os.listdir(src_path)):
            if name.endswith('.mtf') and (not letters or name[0].lower() in letters):
                targets.append(name)

    if not targets:
        print(f'[提示] 在目录 {src_path} 中未找到任何待处理的 .mtf 文件。')
        return

    # 预计算总行数
    total_lines = 0
    valid_targets = []
    for name in targets:
        try:
            path = os.path.join(src_path, name)
            pairs = file_io.read_pairs(path)
            total_lines += sum(1 for _, lhs, rhs in pairs if lhs and rhs)
            valid_targets.append(name)
        except Exception as e:
            print(f"[警告] 读取文件 {name} 失败，将跳过此文件。原因: {e}")
    targets = valid_targets

    print(f"[开始] 待处理文件 {len(targets)} 个，总计 {total_lines} 行。")

    # 干跑模式
    if args.dry_run:
        print("[DRY RUN] 模式，仅列出文件，不执行操作:")
        for name in targets:
            print(f"  - {name}")
        return

    # --- 2. 初始化状态 ---
    start_time = time.time()
    run_ts = int(start_time)
    
    # 为本次运行创建独立的日志目录
    run_log_dir = os.path.join(PROJECT_ROOT, args.log_dir, time.strftime('%Y%m%d-%H%M%S'))
    os.makedirs(run_log_dir, exist_ok=True)
    print(f"[日志] 本次运行日志目录: {run_log_dir}")

    # 术语表输出目录
    glossary_out_dir = os.path.join(PROJECT_ROOT, args.glossary_out, time.strftime('%Y%m%d-%H%M%S'))

    # 将 args 转换为字典以传递给 runner
    config = vars(args)
    config['log_dir'] = run_log_dir

    # 全局进度跟踪
    progress = {
        'start_time': start_time,
        'last_update_time': start_time,
        'total_lines': float(total_lines),
        'files_total': float(len(targets)),
        'done': 0.0,
        'ok': 0.0,
        'fail': 0.0,
        'files_done': 0.0,
        'fail_list': [],
        'glossary_in_dirs': [os.path.join(PROJECT_ROOT, d.strip()) for d in args.glossary_in.split(',')],
        'mine_terms': args.mine_terms,
        'terms_map': {},
    }
    
    # 全局用量统计
    usage_acc: Dict[str, float] = {}

    # --- 3. 执行处理 ---
    grand_ok = grand_fail = 0
    for file_name in targets:
        file_path = os.path.join(src_path, file_name)
        # 注意：这里我们将 PROJECT_ROOT 作为根目录传递
        ok, fail = runner.process_file(PROJECT_ROOT, file_path, api_key, config, progress, usage_acc)
        grand_ok += ok
        grand_fail += fail

    # --- 4. 结束与总结 ---
    elapsed_time = time.time() - start_time
    print(f"\n[完成] 总耗时: {elapsed_time:.2f} 秒。")

    # 费用估算
    hit = usage_acc.get('prompt_cache_hit_tokens', 0.0)
    miss = usage_acc.get('prompt_cache_miss_tokens', 0.0)
    comp = usage_acc.get('completion_tokens', 0.0)
    cost = (hit / 1e6 * 0.2) + (miss / 1e6 * 2.0) + (comp / 1e6 * 3.0)
    print(f"[用量] 成功 {grand_ok} 行，失败 {grand_fail} 行。估算: cache_hit={int(hit)}, cache_miss={int(miss)}, output={int(comp)}。预估费用 ≈ ¥{cost:.4f}")

    # 失败摘要
    if progress['fail_list']:
        print('\n[失败摘要] 以下行未成功写入:')
        by_file: Dict[str, List[Dict[str, Any]]] = {}
        for item in progress['fail_list']:
            by_file.setdefault(item['file'], []).append(item)
        for fname, items in by_file.items():
            print(f"  - {fname}: 共 {len(items)} 行")
            for it in sorted(items, key=lambda x: int(x['index'])):
                print(f"      行 {it['index']}: {it['reason']} | LHS: {it['lhs'][:80]}")

    # 写出挖掘的术语
    if args.mine_terms and progress['terms_map']:
        print("\n[术语] 正在写出挖掘到的术语候选...")
        glossary_tool.write_mined_terms(glossary_out_dir, progress['terms_map'], run_ts)

if __name__ == '__main__':
    main()
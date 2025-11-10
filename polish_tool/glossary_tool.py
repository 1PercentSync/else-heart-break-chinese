# -*- coding: utf-8 -*-
"""
术语表工具模块。

功能:
- 加载外部 CSV 格式的术语表文件。
- 在待翻译文本中查找匹配的术语。
- 从原文和译文中自动挖掘候选术语（--mine-terms 功能）。
- 将挖掘出的术语写出到 CSV 文件以供人工审核。
"""
import regex as re
import os
from typing import List, Tuple, Dict, Set, Optional, Any

# 定义瑞典语和英语的停用词，用于在术语挖掘时过滤掉常用但无意义的词汇
_SV_STOP = set('''och det att som en jag på är av för med han till den i inte ett var mig sig men om här de då vi ni er dig sin detta dessa eller vad vilken vilka där varför hur när från över under också bara redan alltså ännu samt hos mot utan mellan inom runt genom innan efter eftersom'''.split())
_EN_STOP = set('''the a an and or of to in on at for with by from into over under between within through after before since so that as is are was were be been being do does did not no nor but if then else when where why how this these those here there you i he she it we they me him her us them my your his her its our their mine yours hers ours theirs who whom whose which what'''.split())

def load_glossary_inputs(paths: List[str]) -> List[Tuple[str, str, str]]:
    """从指定目录加载所有 .csv 术语表文件。

    输入:
        paths (List[str]): 包含 .csv 文件的目录路径列表。

    输出:
        List[Tuple[str, str, str]]: 一个元组列表，每项包含 (瑞典语原文, 英语译文, 中文译文)。
    """
    out: List[Tuple[str, str, str]] = []
    for p in paths:
        if not p or not os.path.isdir(p):
            continue
        for name in os.listdir(p):
            if not name.lower().endswith('.csv'):
                continue
            fp = os.path.join(p, name)
            try:
                with open(fp, 'r', encoding='utf-8-sig') as f: # utf-8-sig 兼容带 BOM 的文件
                    header = f.readline() # 跳过表头
                    for line in f:
                        parts = [x.strip() for x in line.rstrip('\n').split(',')]
                        if len(parts) < 3:
                            continue
                        src, tgt_en, tgt_zh = parts[0], parts[1], parts[2]
                        if src and (tgt_en or tgt_zh):
                            out.append((src, tgt_en, tgt_zh))
            except Exception:
                continue # 单个文件读取失败不影响其他
    return out

def find_glossary_matches(lhs: str, glossary: List[Tuple[str, str, str]], max_items: int = 10) -> List[Dict[str, str]]:
    """在给定的瑞典语文本中查找术语表匹配项。

    输入:
        lhs (str): 待检查的瑞典语原文。
        glossary (List[Tuple[str, str, str]]): 已加载的术语表。
        max_items (int): 最多返回的匹配项数量。

    输出:
        List[Dict[str, str]]: 匹配到的术语列表，每项是一个包含 source_swe, target_en, target_zh 的字典。
    """
    matches: List[Dict[str, str]] = []
    for src, en, zh in glossary:
        if src and src in lhs:
            matches.append({'source_swe': src, 'target_en': en, 'target_zh': zh})
            if len(matches) >= max_items:
                break
    return matches


# --- 术语挖掘相关函数 ---

def _is_sentence_like(s: str) -> bool:
    """判断一个字符串是否像一个完整的句子，用于过滤挖掘出的非术语长句。"""
    s = s.strip()
    if not s: return True
    if len(s) > 40: return True # 长度过长
    if s.count(' ') >= 4: return True # 空格过多
    if any(p in s for p in ['\n', '\t']): return True # 包含换行/制表符
    if any(p in s[:-1] for p in ['.', '!', '?', '。', '！', '？']): return True # 中间包含句末标点
    return False

def _clean_token(tok: str) -> str:
    """清理单个候选词，移除标点、占位符和数字。"""
    t = tok.strip().strip('"\'`“”‘’,.?!:;()[]{}<>')
    if any(x in t for x in ['%s', '%d', '{', '}', '<', '>', '[', ']', '\\']):
        return ''
    if any(ch.isdigit() for ch in t):
        return ''
    return t

def _extract_terms_from_text(text: str, lang: str) -> List[str]:
    """从单段文本中提取候选术语（单词和短语）。"""
    stop_words = _SV_STOP if lang == 'sv' else _EN_STOP
    word_pattern = r"[\p{L}A-Za-zÀ-ÖØ-öø-ÿÅÄÖåäö]+(?:[-'][\p{L}A-Za-zÀ-ÖØ-öø-ÿÅÄÖåäö]+)*"
    
    out: List[str] = []
    # 1. 提取单个单词
    for w in re.findall(word_pattern, text):
        t = _clean_token(w)
        if t and len(t) > 1 and t.lower() not in stop_words:
            out.append(t)
    
    # 2. 提取短语 (连续大写开头的词组)
    phrase_pattern = r"(?:[A-ZÅÄÖ][\wÀ-ÖØ-öø-ÿåäö]+(?:\s+(?:of|av|i|på|och|the|and)\s+)?){1,3}"
    for p in re.findall(phrase_pattern, text):
        p_cleaned = ' '.join(seg for seg in p.split() if seg.lower() not in {'of', 'av', 'i', 'på', 'och', 'the', 'and'})
        p_final = _clean_token(p_cleaned)
        if p_final and not _is_sentence_like(p_final) and p_final not in out:
            out.append(p_final)

    # 3. 提取全大写缩写 (仅英语)
    if lang == 'en':
        for ac in re.findall(r"\b[A-Z]{2,}\b", text):
            if ac not in out:
                out.append(ac)
    return out

def split_rhs_en_zh(rhs: str) -> Tuple[str, str]:
    """将 RHS 分割为英文和中文两部分。"""
    idx = -1
    for i, ch in enumerate(rhs):
        if '\u4e00' <= ch <= '\u9fff':
            idx = i
            break
    if idx == -1:
        return rhs.strip(), ''
    return rhs[:idx].strip(), rhs[idx:].strip()

def mine_terms_from_jobs(jobs: List[Tuple[int, str, str]], file_name: str, terms_map: Dict[Tuple[str, str], Dict[str, Any]]):
    """从待处理的任务列表中挖掘术语，并更新全局术语地图。

    输入:
        jobs: 待处理的行列表 (index, lhs, rhs)。
        file_name: 当前文件名。
        terms_map: 用于累计术语信息的字典。
    """
    for _idx, lhs, rhs in jobs:
        # 提取瑞典语术语
        for term in _extract_terms_from_text(lhs, 'sv'):
            key = (term, 'sv')
            rec = terms_map.setdefault(key, {'count': 0, 'files': set(), 'examples': []})
            rec['count'] += 1
            rec['files'].add(file_name)
            if len(rec['examples']) < 3:
                rec['examples'].append(lhs[:80])
        
        # 提取英语术语
        en_part, _ = split_rhs_en_zh(rhs)
        if en_part:
            for term in _extract_terms_from_text(en_part, 'en'):
                key = (term, 'en')
                rec = terms_map.setdefault(key, {'count': 0, 'files': set(), 'examples': []})
                rec['count'] += 1
                rec['files'].add(file_name)
                if len(rec['examples']) < 3:
                    rec['examples'].append(en_part[:80])

def _csv_quote(cell: str) -> str:
    """为 CSV 单元格内容添加必要的引号和转义。"""
    if any(c in cell for c in [',', '"', '\n']):
        return '"' + cell.replace('"', '""') + '"'
    return cell

def write_mined_terms(out_dir: str, terms_map: Dict[Tuple[str, str], Dict[str, Any]], start_ts: int):
    """将挖掘出的术语候选列表写入 CSV 文件。

    输入:
        out_dir (str): 输出目录。
        terms_map: 包含所有术语信息的字典。
        start_ts (int): 脚本开始运行的时间戳，用于文件名。
    """
    try:
        os.makedirs(out_dir, exist_ok=True)
        out_path = os.path.join(out_dir, f"glossary_candidates_{start_ts}.csv")
        
        with open(out_path, 'w', encoding='utf-8') as f:
            f.write('term,lang,count,files,examples\n')
            
            # 按出现次数降序、术语字母序排序
            sorted_items = sorted(terms_map.items(), key=lambda kv: (-kv[1]['count'], kv[0][0].lower()))
            
            for (term, lang), rec in sorted_items:
                if _is_sentence_like(term):
                    continue # 过滤掉看起来像句子的条目
                
                files_str = ';'.join(sorted(list(rec['files'])))
                examples_str = ' | '.join(rec['examples'])
                
                f.write(','.join([
                    _csv_quote(term),
                    _csv_quote(lang),
                    str(rec['count']),
                    _csv_quote(files_str),
                    _csv_quote(examples_str),
                ]) + '\n')
        print(f"[术语] 已输出术语候选文件：{out_path}")
    except Exception as e:
        print(f"[错误] 写出术语候选文件失败：{e}")

# -*- coding: utf-8 -*-
"""
文本处理与校验模块。

功能:
- 提供一系列用于清洗、规范化和校验译文（RHS）的函数。
- 确保输出的文本符合项目要求。
"""
import re
import os
import json
from typing import List, Tuple, Optional, Set

# 从 .config 模块导入默认规则
from .config import DEFAULT_CANON_RULES

def get_placeholders(s: str) -> Set[str]:
    """从字符串中提取所有类型的占位符。

    支持的占位符格式: %s, %d, {name}, {0}, <tag>, [link], \n, \t

    输入:
        s (str): 待检查的字符串。

    输出:
        Set[str]: 包含所有找到的占位符的集合，自动去重。
    """
    pats: List[str] = []
    pats += re.findall(r'%[sdif]', s)
    pats += re.findall(r'\{[^}]+\}', s)
    pats += re.findall(r'<[^>]+>', s)
    pats += re.findall(r'\[[^\]]+\]', s)
    pats += re.findall(r'\\[nt]', s)
    return set(pats)

def sanitize_rhs(rhs: str) -> Optional[str]:
    """对模型返回的原始 RHS 字符串进行清洗和初步校验。

    清洗规则:
    - 移除首尾的空白字符和零宽空格。
    - 剥离首尾的引号。
    - 将换行符和回车符替换为空格。
    - 移除中文句末的句号 "。"。
    - 拒绝包含不规范内容（如代码块标记、JSON结构、中文引号）的字符串。

    输入:
        rhs (str): 从模型获取的原始译文。

    输出:
        Optional[str]: 清洗后的字符串。如果字符串无效或不合规，则返回 None。
    """
    s = rhs.strip().strip('\u200b')
    if (s.startswith('"') and s.endswith('"')) or (s.startswith("'") and s.endswith("'")):
        s = s[1:-1]
    
    s = s.replace('\r', ' ').replace('\n', ' ').strip()
    
    if s.endswith('。'):
        s = s[:-1]
    
    if not s:
        return None
        
    # 拒绝包含代码块、JSON样式或中文引号的响应
    if '```' in s or s.startswith('[') or s.startswith('{') or '\"index\"' in s or '\\\"index\\\"' in s:
        return None
    if '“' in s or '”' in s or '‘' in s or '’' in s:
        return None
        
    return s

def load_canonical_rules(root: str) -> List[Tuple[str, str]]:
    """加载术语规范化规则。

    首先尝试从 tools/canonical_rules.json 加载自定义规则。
    如果文件不存在或加载失败，则使用 config.py 中定义的默认规则。

    输入:
        root (str): 项目的根目录路径。

    输出:
        List[Tuple[str, str]]: 一个元组列表，每项包含 (待匹配的正则表达式, 用于替换的字符串)。
    """
    cfg_path = os.path.join(root, 'tools', 'canonical_rules.json')
    try:
        if os.path.isfile(cfg_path):
            with open(cfg_path, 'r', encoding='utf-8') as f:
                data = json.load(f)
            
            rules = []
            for item in data:
                pat = item.get('pattern')
                rep = item.get('repl')
                if isinstance(pat, str) and isinstance(rep, str):
                    rules.append((pat, rep))
            
            # 如果自定义规则文件有效但为空，则返回默认规则
            return rules or DEFAULT_CANON_RULES
    except Exception:
        # 任何异常都回退到默认规则
        pass
    return DEFAULT_CANON_RULES

def canonicalize_rhs(rhs: str, rules: List[Tuple[str, str]]) -> str:
    """应用规范化规则来统一 RHS 中的术语和表达。

    输入:
        rhs (str): 待处理的译文。
        rules (List[Tuple[str, str]]): 生效的规范化规则列表。

    输出:
        str: 规范化处理后的译文。
    """
    out = rhs
    for pat, rep in rules:
        out = re.sub(pat, rep, out)
    return out

def contains_swedish_words(lhs: str, rhs: str) -> bool:
    """检查 RHS 是否可能错误地包含了 LHS 中的瑞典语单词。

    这是一个启发式检查，用于捕捉模型可能发生的“翻译搬运”错误。
    它会提取 LHS 中长度大于等于3的单词，并检查它们是否存在于 RHS 中。

    输入:
        lhs (str): 左侧的瑞典语原文。
        rhs (str): 右侧的译文。

    输出:
        bool: 如果检测到可能的瑞典语单词，返回 True，否则返回 False。
    """
    # 正则表达式匹配所有瑞典语特有字符以及标准拉丁字母
    tokens = re.findall(r"[A-Za-zÅÄÖåäö]{3,}", lhs)
    for t in tokens:
        if t in rhs:
            return True
    return False

# -*- coding: utf-8 -*-
"""
文件读写模块。

功能:
- 封装对 .mtf 文件的特定格式的读写操作。
- 提供通用的文件内容读取功能。
"""
import re
from typing import List, Tuple

# 定义用于解析 .mtf 文件中键值对的正则表达式
# 格式: "key" => "value"
PAIR_RE = re.compile(r'^"((?:[^"\\]|\\.)*)"\s*=>\s*"((?:[^"\\]|\\.)*)"\s*$')

def read_pairs(path: str) -> List[Tuple[str, str, str]]:
    """读取 .mtf 文件，解析为 (原始行, 左侧瑞典语, 右侧译文) 的元组列表。

    输入:
        path (str): .mtf 文件的绝对路径。

    输出:
        List[Tuple[str, str, str]]: 一个列表，每个元素是一个元组，包含:
            - 原始行内容 (str)
            - 左侧瑞典语 (str)，如果不是键值对格式则为空字符串
            - 右侧当前译文 (str)，如果不是键值对格式则为空字符串
    """
    items: List[Tuple[str, str, str]] = []
    with open(path, 'r', encoding='utf-8') as f:
        for line in f:
            m = PAIR_RE.match(line.rstrip('\n'))
            if m:
                # 如果匹配成功，存储 (原始行, key, value)
                items.append((line, m.group(1), m.group(2)))
            else:
                # 如果不匹配（如空行或注释），key 和 value 为空
                items.append((line, '', ''))
    return items

def write_pairs(path: str, items: List[Tuple[str, str, str]]):
    """将润色后的内容写回 .mtf 文件。

    输入:
        path (str): 目标 .mtf 文件的绝对路径。
        items (List[Tuple[str, str, str]]): 包含待写入内容的元组列表。
    """
    with open(path, 'w', encoding='utf-8', newline='\n') as f:
        for line, lhs, rhs in items:
            if lhs == '' and rhs == '':
                # 如果是原始的非键值对行，直接写回
                f.write(line)
            else:
                # 否则，按格式写回键值对
                f.write(f'\"{lhs}\" => \"{rhs}\"\n')

def read_text_file(p: str) -> str:
    """读取一个纯文本文件。

    输入:
        p (str): 文件路径。

    输出:
        str: 文件内容。如果读取失败，返回空字符串。
    """
    try:
        with open(p, 'r', encoding='utf-8') as f:
            return f.read()
    except Exception:
        return ''

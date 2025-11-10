# -*- coding: utf-8 -*-
"""
提示工程模块 (Prompt Engineering)。

功能:
- 集中管理和构造与大模型交互所需的 System 和 User 提示。
- 根据不同的模式（如 full/lite/auto）和上下文动态生成提示内容。
"""
import os
import json
from typing import Optional, Dict, Any, List, Tuple

# 依赖于 file_io 模块来读取背景文件
from . import file_io

def build_system_prompt(root: str, bg_mode: str = 'full', file_name: Optional[str] = None) -> str:
    """构造发送给模型的 System 提示。

    System 提示包含了模型的角色、核心指令、格式要求以及背景信息。

    输入:
        root (str): 项目根目录，用于定位背景信息文件。
        bg_mode (str): 背景信息模式 ('full', 'lite', 'auto')。
        file_name (Optional[str]): 当前处理的文件名，用于 'auto' 模式的判断。

    输出:
        str: 构造完成的 System 提示字符串。
    """
    bg_path = os.path.join(root, '背景信息')
    t1 = file_io.read_text_file(os.path.join(bg_path, '背景信息汇总.md'))
    t2 = file_io.read_text_file(os.path.join(bg_path, '剧情与翻译参考详解.md'))
    t3 = file_io.read_text_file(os.path.join(bg_path, '专有名词与翻译对照.md'))
    
    rules = (
        '你是一名资深游戏本地化编辑，负责 Else Heart.Break() 的文本润色。\n'
        '务必遵守：\n'
        '1) 左侧瑞典语绝对不可改动；你只输出右侧译文。\n'
        '2) 右侧为双语：英文在前，中文在后（以单个空格分隔）。\n'
        '3) 不使用中文引号；中文句末不加中文句号“。”（问号/叹号/省略号保留）。\n'
        '4) 保留占位/标记：%s、%d、{name}、{0}、<tag>、[link]、\\n、\\t 等。\n'
        '5) 专名与口径统一：Wellspring、Devotchka、SPRAK、GRIMM、Slurp()、Queen of the Internet、kronor 等。\n'
        '6) 只输出 JSON 对象：{"rhs": "英文在前 中文在后"}，不要输出任何说明/代码块。\n'
        '\n【术语与风格统一（重要）】\n'
        '- “rejv / rave” 统一译为 “rave 狂欢派对”，避免音译“锐舞派对”。\n'
        '- 口语 “Dope” 译为 “很酷/真棒”，避免误译为“傻瓜”。\n'
        '- 如遇编程/数学/范畴术语（如 Kleisli），英文侧用标准术语，中文侧加简洁括注。\n'
        '- 同一文件内术语保持一致，不在相邻行混用不同表达。\n'
    )

    # 根据 bg_mode 决定背景信息的详略
    use_full = True
    if bg_mode == 'lite':
        use_full = False
    elif bg_mode == 'auto':
        base = (file_name or '')
        if '.' in base:
            base = base.split('.', 1)[0]
        # 简单启发式：UI/菜单/教学/系统类文件名通常用 lite 模式以节约成本
        key = base.lower()
        if key.startswith('_') or 'phone' in key or 'teach' in key or 'siri' in key:
            use_full = False
        else:
            use_full = True

    if use_full:
        ctx = (
            "\n【背景信息：每次均附带，保持口径一致】\n"
            "=== 背景信息汇总.md ===\n" + t1 +
            "\n=== 剧情与翻译参考详解.md ===\n" + t2 +
            "\n=== 专有名词与翻译对照.md ===\n" + t3
        )
    else:
        # 精简版：只附专有名词对照，节约 token
        ctx = (
            "\n【背景信息（精简，仅专名对照）】\n"
            "=== 专有名词与翻译对照.md ===\n" + t3
        )
        
    return rules + ctx

def _get_context_hint(file_name: str) -> str:
    """从文件名中提取说话者和场景线索。"""
    base = file_name.split('.', 1)[0]
    speaker, scene = '', ''
    if base.startswith('_'):
        scene = base.lstrip('_')
    else:
        parts = base.split('_', 1)
        speaker = parts[0]
        scene = parts[1] if len(parts) > 1 else ''
    return f"Speaker: {speaker or '-'}; Scene: {scene or '-'}"

def build_user_prompt(file_name: str, lhs: str, rhs_current: str, glossary_matches: Optional[List[Dict[str, str]]] = None) -> str:
    """为单行翻译任务构造 User 提示（JSON 格式）。

    输入:
        file_name (str): 当前文件名。
        lhs (str): 左侧瑞典语原文。
        rhs_current (str): 右侧当前译文。
        glossary_matches (Optional[List[Dict[str, str]]]): 匹配到的术语表条目。

    输出:
        str: 构造完成的 User 提示字符串（JSON 格式）。
    """
    hint = _get_context_hint(file_name)
    instruction = (
        '请润色右侧译文 rhs，使其自然、符合口径，保持双语顺序（英文在前 中文在后）。\n'
        '输出严格 JSON 对象：{"rhs": "..."}，不要返回代码块/数组/多余文字。\n'
        '保留占位/标签/转义，不要改动左侧瑞典语。\n'
        '若涉及编程/数学/范畴等专业术语，请使用标准学术术语，并在中文处给出简洁括注解释；\n'
        '例如："Kleislipilar" → "Kleisli arrows … Kleisli 箭（范畴论）…"；不要把瑞典语术语直接搬到右侧。\n'
        f'注意：文件名中包含当前说话者/场景线索（{hint}），请在口吻与词汇上与该说话者和场景保持一致。'
    )
    if glossary_matches:
        instruction += '\n' + '若句子包含以下术语/短语，请采用给定译法（英文在前 中文在后）。'

    payload: Dict[str, Any] = {
        "file": file_name,
        "instruction": instruction,
        "lhs_swedish": lhs,
        "rhs_current": rhs_current,
        "context_hint": hint,
        "expect": {"rhs": "英文在前 中文在后"},
        "glossary": glossary_matches or []
    }
    return json.dumps(payload, ensure_ascii=False)

def build_batch_user_prompt(file_name: str, batch: List[Tuple[int, str, str]], batch_glossary: Optional[List[Dict[str, str]]] = None) -> str:
    """为批量翻译任务构造 User 提示（JSON 格式）。

    输入:
        file_name (str): 当前文件名。
        batch (List[Tuple[int, str, str]]): 包含 (行号, lhs, rhs) 的元组列表。
        batch_glossary (Optional[List[Dict[str, str]]]): 适用于该批次的术语表条目并集。

    输出:
        str: 构造完成的批量任务 User 提示字符串（JSON 格式）。
    """
    hint = _get_context_hint(file_name)
    items = [{
        'index': i,
        'lhs_swedish': lhs,
        'rhs_current': rhs
    } for (i, lhs, rhs) in batch]
    
    instruction = (
        '请批量润色以下行的右侧 rhs，保持双语顺序（英文在前 中文在后）。\n'
        '返回严格 JSON 对象：{"items": [{"index": number, "rhs": "..."}, ...]}；不要返回代码块/多余文字。\n'
        '保留占位/标签/转义，不要改动左侧瑞典语。\n'
        f'注意：文件名包含说话者/场景线索（{hint}），请保持对应口吻与词汇。'
    )
    if batch_glossary:
        instruction += '\n' + '若句子包含以下术语/短语，请采用给定译法。'

    user_payload: Dict[str, Any] = {
        'file': file_name,
        'instruction': instruction,
        'context_hint': hint,
        'items': items,
        'glossary': batch_glossary or []
    }
    return json.dumps(user_payload, ensure_ascii=False)

def build_retry_user_prompt(file_name: str, lhs: str, rhs_current: str) -> str:
    """为包含瑞典语单词的失败行构造重试 User 提示。

    在标准 User 提示的基础上，增加额外的强化指令，要求模型修正“翻译搬运”错误。

    输入:
        file_name (str): 当前文件名。
        lhs (str): 左侧瑞典语原文。
        rhs_current (str): 右侧当前译文。

    输出:
        str: 构造完成的重试 User 提示字符串（JSON 格式）。
    """
    # 复用标准的单行提示构造逻辑
    base_payload = json.loads(build_user_prompt(file_name, lhs, rhs_current))
    
    # 增加额外指令
    extra_instruction = (
        '\n【重试指令】上一轮输出可能包含瑞典语词条或不规范术语。\n'
        '请避免把瑞典语原词搬到右侧；若为专有术语，请用标准英文术语，并在中文处给出简洁括注。\n'
        '例如：rejv → rave 狂欢派对；Kleislipilar → Kleisli arrows … Kleisli 箭（范畴论）。\n'
    )
    base_payload['instruction'] = str(base_payload.get('instruction', '')) + extra_instruction
    
    return json.dumps(base_payload, ensure_ascii=False)

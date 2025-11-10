# -*- coding: utf-8 -*-
"""
日志记录与进度显示工具模块。

功能:
- 提供行级别的详细日志记录功能，将每次调用的关键信息保存为 JSON 文件。
- 格式化 ETA（预计剩余时间）字符串。
- 清洗文件名以便安全地用于创建日志文件。
"""
import os
import re
import json
import time
from typing import Optional, Dict, Any

def sanitize_filename(s: str) -> str:
    """移除文件名中的非法字符，替换为空格，以确保可以安全地创建文件。"""
    return re.sub(r'[^\w\-\.]+', '_', s)

def write_line_log(
    log_dir: str, 
    file_name: str, 
    idx: int, 
    lhs: str, 
    rhs_current: str,
    system_prompt: Optional[str], 
    user_prompt: Optional[str], 
    raw_response: Optional[str], 
    parsed_rhs: Optional[str],
    usage: Optional[Dict[str, int]],
    error: Optional[str]
) -> None:
    """将单行处理的详细信息写入一个独立的 JSON 日志文件。

    每个日志文件都以时间戳、文件名和行号命名，确保唯一性和可追溯性。
    """
    try:
        os.makedirs(log_dir, exist_ok=True)
        ts = int(time.time() * 1000)
        base_name = f"{ts}_{sanitize_filename(file_name)}_{idx}"
        log_path = os.path.join(log_dir, base_name + '.json')
        
        with open(log_path, 'w', encoding='utf-8') as f:
            json.dump({
                'file': file_name,
                'index': idx,
                'lhs': lhs,
                'rhs_current': rhs_current,
                'system_prompt': system_prompt,
                'user_prompt': user_prompt,
                'raw_response': raw_response,
                'parsed_rhs': parsed_rhs,
                'usage': usage,
                'error': error,
            }, f, ensure_ascii=False, indent=2)
    except Exception as e:
        # 日志写入失败不应中断主流程，仅在控制台打印错误
        print(f"[警告] 写入行日志失败: {e}")

def format_eta(seconds: float) -> str:
    """将秒数格式化为 HH:MM:SS 格式的字符串。"""
    if seconds <= 0 or seconds == float('inf'):
        return '--:--:--'
    m, s = divmod(int(seconds), 60)
    h, m = divmod(m, 60)
    return f"{h:02d}:{m:02d}:{s:02d}"

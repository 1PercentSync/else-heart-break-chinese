# -*- coding: utf-8 -*-
"""
API 客户端模块。

功能:
- 封装对 DeepSeek API 的调用逻辑。
- 支持单行（line）和批量（batch）两种模式的请求。
- 处理 API 返回的数据，包括 JSON 解析和错误处理。
- 内置重试机制和对 requests 库的依赖检查。
"""
import re
import json
import time
from typing import List, Tuple, Dict, Optional, Any

# 尝试导入 requests 库，如果失败则设置为 None，并在调用时抛出异常
try:
    import requests
except ImportError:
    requests = None

def _parse_response(raw_text: str) -> Optional[Dict[str, Any]]:
    """从模型返回的原始文本中解析出 JSON 对象。

    支持的格式:
    - 纯 JSON 字符串。
    - 包含在 Markdown 代码块 (```json ... ```) 中的 JSON。
    - 尝试从可能被截断或包含额外文本的字符串中提取最外层的 {}。

    输入:
        raw_text (str): 模型返回的原始字符串。

    输出:
        Optional[Dict[str, Any]]: 解析成功则返回字典，否则返回 None。
    """
    text = raw_text.strip()
    # 提取 Markdown 代码块中的内容
    if "```" in text:
        match = re.search(r"```(?:json)?\s*([\s\S]*?)```", text, re.IGNORECASE)
        if match:
            text = match.group(1).strip()
    
    try:
        # 尝试直接解析
        return json.loads(text)
    except json.JSONDecodeError:
        # 如果失败，尝试寻找最外层的花括号来补救
        if '{' in text and '}' in text:
            start = text.find('{')
            end = text.rfind('}')
            if end > start:
                slice_to_try = text[start:end+1]
                try:
                    return json.loads(slice_to_try)
                except json.JSONDecodeError:
                    return None # 补救失败
    return None

def call_deepseek_line(
    api_key: str, 
    system_prompt: str, 
    user_prompt: str,
    timeout: int, 
    temperature: float, 
    max_tokens: int,
    session: Optional['requests.Session'] = None
) -> Tuple[str, str, Dict[str, int]]:
    """以单行模式调用 DeepSeek API。

    输入:
        api_key (str): DeepSeek API 密钥。
        system_prompt (str): 系统提示。
        user_prompt (str): 用户提示 (JSON 字符串)。
        timeout (int): 请求超时时间（秒）。
        temperature (float): 模型温度。
        max_tokens (int): 最大生成 token 数。
        session (Optional[requests.Session]): 可复用的 requests 会话对象。

    输出:
        Tuple[str, str, Dict[str, int]]: 一个元组，包含:
            - 解析后的 RHS 译文 (str)
            - 模型的原始响应文本 (str)
            - 用量信息字典 (Dict)

    异常:
        RuntimeError: 如果 requests 库未安装或 API 多次调用失败。
        ValueError: 如果 API 返回的不是预期的 JSON 结构。
    """
    if not requests:
        raise RuntimeError("缺少 requests 库，请运行 'pip install requests' 进行安装。")

    url = "https://api.deepseek.com/chat/completions"
    headers = {"Authorization": f"Bearer {api_key}", "Content-Type": "application/json"}
    payload = {
        "model": "deepseek-chat",
        "messages": [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": user_prompt}
        ],
        "response_format": {"type": "json_object"},
        "temperature": temperature,
        "max_tokens": max_tokens,
        "stream": False,
    }
    
    sess = session or requests
    # 简单的重试机制
    for attempt in range(3):
        try:
            r = sess.post(url, headers=headers, json=payload, timeout=timeout)
            if r.status_code >= 500:
                time.sleep(1.0 + attempt * 0.8) # 服务器错误，等待后重试
                continue
            r.raise_for_status() # 触发非 5xx 的 HTTP 错误
            
            data = r.json()
            raw_response_text = data.get("choices", [{}])[0].get("message", {}).get("content", "")
            usage = data.get('usage', {}) or {}

            parsed_obj = _parse_response(raw_response_text)
            if not isinstance(parsed_obj, dict) or 'rhs' not in parsed_obj or not isinstance(parsed_obj['rhs'], str):
                raise ValueError(f"模型未返回形如 {{ \"rhs\": \"...\" }} 的 JSON 对象。原始响应: {raw_response_text[:200]}")
            
            return parsed_obj['rhs'], raw_response_text, usage
        except requests.RequestException as e:
            if attempt == 2: # 最后一次尝试仍然失败
                raise RuntimeError(f"API 请求失败: {e}")
            time.sleep(1.0)

    raise RuntimeError("API 请求在多次重试后仍然失败。")

def call_deepseek_batch(
    api_key: str, 
    system_prompt: str, 
    user_prompt: str,
    timeout: int, 
    temperature: float, 
    max_tokens: int,
    session: Optional['requests.Session'] = None
) -> Tuple[Dict[int, str], str, Dict[str, int]]:
    """以批量模式调用 DeepSeek API。

    输入:
        (同上)

    输出:
        Tuple[Dict[int, str], str, Dict[str, int]]: 一个元组，包含:
            - 一个字典，映射 {行号: RHS 译文} (Dict)
            - 模型的原始响应文本 (str)
            - 用量信息字典 (Dict)
    """
    if not requests:
        raise RuntimeError("缺少 requests 库，请运行 'pip install requests' 进行安装。")

    url = "https://api.deepseek.com/chat/completions"
    headers = {"Authorization": f"Bearer {api_key}", "Content-Type": "application/json"}
    payload = {
        "model": "deepseek-chat",
        "messages": [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": user_prompt}
        ],
        "response_format": {"type": "json_object"},
        "temperature": temperature,
        "max_tokens": max_tokens,
        "stream": False,
    }

    sess = session or requests
    r = sess.post(url, headers=headers, json=payload, timeout=timeout)
    r.raise_for_status()
    
    data = r.json()
    raw_response_text = data.get("choices", [{}])[0].get("message", {}).get("content", "")
    usage = data.get('usage', {}) or {}

    parsed_obj = _parse_response(raw_response_text)
    if not isinstance(parsed_obj, dict) or 'items' not in parsed_obj or not isinstance(parsed_obj['items'], list):
        raise ValueError(f'批量模式返回的不是 {{ "items": [...] }} 结构。原始响应: {raw_response_text[:200]}')

    result_map: Dict[int, str] = {}
    for item in parsed_obj['items']:
        if isinstance(item, dict) and 'index' in item and 'rhs' in item:
            result_map[int(item['index'])] = str(item['rhs'])
            
    return result_map, raw_response_text, usage

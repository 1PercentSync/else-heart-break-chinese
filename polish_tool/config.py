# -*- coding: utf-8 -*-
"""
配置模块。

功能:
- 存放项目范围内的常量和默认配置。
- 使配置与业务逻辑分离，方便调整。

主要内容:
- DEFAULT_CANON_RULES: 默认的术语规范化规则。
"""
from typing import List, Tuple

# 术语规范与俗语统一（可外部配置覆盖）
# 格式为 (正则表达式, 替换字符串)
DEFAULT_CANON_RULES: List[Tuple[str, str]] = [
  # 音译/不统一 → 统一
  (r"锐舞(派对)?", "狂欢派对"),
  (r"酷毙", "很酷"),
]

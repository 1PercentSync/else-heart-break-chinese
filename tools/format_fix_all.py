from __future__ import annotations
import re
from pathlib import Path

"""
用途：
  - 批量校正 English 目录下所有 .mtf 文本中箭头右侧（译文）的格式问题，仅修改右侧内容，左侧原文（瑞典语）不改。

规范（严格遵守）：
  1) 中文内容不能被三种引号括起：“ ” 和 ' '
     示例问题：'Ja?' => 'Yes? “什么事？”'
  2) 翻译内容句末不能带中文句号“。”（机器翻译常见多余标点）
     示例问题：'Mm' => 'Mm 嗯。'
  3) 箭头 => 右侧的引号必须完整，整行保持 "…" => "…" 的成对结构，避免游戏解析报错
     示例问题：'Mm' => 'Mm 嗯

实现说明：
  - 通过正则识别每行的 "…" => "…" 结构；
  - 对右侧译文做三项清理：去掉中文引号、去掉中文句尾“。”、去除贴在中文两侧的单引号；
  - 其他内容（含占位符、英文片段、感叹号/问号等）保持不变。
"""

ROOT = Path(__file__).resolve().parents[1]
EN_DIR = ROOT / "English"

# 匹配一整行的 "…" => "…" 结构（左右各为一个双引号包裹的字符串）
PAIR_RE = re.compile(r'^("(?:[^"\\]|\\.)*")\s*=>\s*("(?:[^"\\]|\\.)*")\s*$')

# 需要去除的中文引号（全角/弯引号/书名号等）
CHINESE_QUOTE_CHARS = "\u201C\u201D\u300C\u300D\u300E\u300F\u00AB\u00BB"


def sanitize_rhs(rhs: str) -> str:
    """按规范清理箭头右侧译文，保证：
    - 不含中文引号（“”《》等）
    - 句末不为中文句号“。”
    - 移除紧贴中文字符两侧的英文单引号
    """
    s = rhs
    # 去除常见中文引号
    s = s.replace("\u201C", "").replace("\u201D", "")
    s = s.replace("\u300C", "").replace("\u300D", "")
    s = s.replace("\u300E", "").replace("\u300F", "")
    s = s.replace("\u00AB", "").replace("\u00BB", "")

    # 去除贴在中文字符两侧的英文单引号（如 '什么事？' -> 什么事？）
    s = re.sub(r"'(?=[\u4e00-\u9fff])", "", s)
    s = re.sub(r"(?<=[\u4e00-\u9fff])'", "", s)

    # 去除首尾空白
    s = s.strip()

    # 句尾为中文句号“。”则移除
    if s.endswith("。"):
        s = s[:-1]

    return s


def process_file(path: Path) -> int:
    """处理单个 .mtf 文件：仅在匹配到的行上替换右侧译文，其余原样输出。"""
    changed = 0
    lines = path.read_text(encoding="utf-8").splitlines()
    out_lines: list[str] = []
    for line in lines:
        m = PAIR_RE.match(line)
        if not m:
            out_lines.append(line)
            continue
        lhs = m.group(1)
        rhs_full = m.group(2)
        # 提取右侧引号内部内容
        rhs_inner = rhs_full[1:-1]
        new_rhs_inner = sanitize_rhs(rhs_inner)
        if new_rhs_inner != rhs_inner:
            changed += 1
        out_lines.append(f"{lhs} => \"{new_rhs_inner}\"")
    if changed:
        path.write_text("\n".join(out_lines) + "\n", encoding="utf-8")
    return changed


def main():
    """遍历 English 下全部 .mtf 文件，逐一应用格式规范。"""
    files = sorted(EN_DIR.glob("*.mtf"), key=lambda p: p.name.lower())
    total_changed = 0
    for p in files:
        total_changed += process_file(p)
    print(f"Processed {len(files)} files, modified {total_changed} lines")


if __name__ == "__main__":
    main()

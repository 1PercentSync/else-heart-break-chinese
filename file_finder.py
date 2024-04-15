import os
import re

def file_accessor(directory: str, suffix: str) -> list:
  '''返回指定目录中指定后缀名文件的路径字符串'''
  return [os.path.abspath(os.path.join(directory, f))
          for f in os.listdir(directory)
          if os.path.isfile(os.path.join(directory, f))
          and f.endswith(suffix)]

def format_print(validation_result: dict) -> None:
  if not any(validation_result):
    print("Not Found Anything")
  else:
    for filename, lines in validation_result.items():
      print(f"File: {filename}\n\tIn line => {lines}\n")

def is_contained_keyword(file_path: str, keyword: str) -> tuple[str, list[int]] | None:
  '''检查某个文件是否包含关键字'''
  valid_lines = []
  with open(file_path, 'r', encoding='utf-8') as file:
    for line_num, line in enumerate(file, start=1):
      if re.search(keyword, line):
        valid_lines.append(line_num)
  if any(valid_lines):
    return os.path.basename(file_path), valid_lines
  return None

if __name__ == '__main__':
  '''
  从某个关键字出发找对应的文件名
  以便于从游戏文本出发找到出问题的翻译文件
  '''
  keyword = input("待查找的关键字是\n>>> ")

  search_result = {}
  for file_path in file_accessor('./English', '.mtf'):
    result = is_contained_keyword(file_path, keyword)
    if result is not None:
      filename, invalid_lines = result
      if filename not in search_result:
        search_result[filename] = []
      search_result[filename].extend(invalid_lines)
  format_print(search_result)

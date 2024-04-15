import os
import re
import sys

def file_accessor(directory: str, suffix: str) -> list[str]:
  '''返回指定目录中指定后缀名文件的路径字符串'''
  return [os.path.abspath(os.path.join(directory, f))
          for f in os.listdir(directory)
          if os.path.isfile(os.path.join(directory, f))
          and f.endswith(suffix)]

def format_print(validation_result: dict) -> None:
  if not any(validation_result):
    print('Files Not Found')
  else:
    for filename, lines in validation_result.items():
      print(f'File: {filename}')
      for line_num, line in lines.items():
        print(f'\t[Line {line_num}]: {line}', end='')
      print()

def validate_file(file_path) -> tuple[str, dict] | None:
  '''校验函数，检测文件内容是否满足正则条件'''
  pattern = re.compile(r'"[^"]*" => "[^"“”]*[^\u4e00-\u9fa5"]*?"')
  '''
  这条正则并不一定能够抓出全部的排版错误，因为我不太懂这个（
  总的来说规则是：
  1. 中文内容不能被三种引号括起：“” ' '
     也就是说像下面这条东西是不该出现的：
     'Ja?' => 'Yes? “什么事？”'
  2. 翻译内容的句末不能有句号（这是机器翻译时擅自添加的），也就是这样：
     'Mm' => 'Mm 嗯。'
  3. 箭头 => 右边的引号必须完整，避免游戏解析出问题，也就是不允许这样：
     'Mm' => 'Mm 嗯
  就三条，其他的只能靠人工抓，不过大部分已经被校对好了
  '''
  invalid_lines = {}
  with open(file_path, 'r', encoding='utf-8') as f:
    for line_num, line in enumerate(f, start=1):
      if not pattern.search(line):
        invalid_lines[line_num] = line
  if any(invalid_lines):
    return os.path.basename(file_path), invalid_lines
  return None

if __name__ == '__main__':
  '''验证校对后的文件是否符合格式'''
  if not sys.stdout.isatty():
    # 使用重定向符时更改字符编码
    sys.stdout.reconfigure(encoding='utf-16')

  validation_result = {}
  for file_path in file_accessor('./English', '.mtf'):
    result = validate_file(file_path)
    if result is not None:
      filename, invalid_lines = result
      validation_result[filename] = invalid_lines
  format_print(validation_result)

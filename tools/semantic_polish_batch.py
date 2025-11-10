from __future__ import annotations
import re
from pathlib import Path

"""
语义润色批处理（仅本批次对照）
- 仅替换箭头右侧译文（英文在前，中文在后）；左侧瑞典语不改。
- 三条格式规则：不使用中文引号；中文结尾不加“。”；右侧引号完整。
运行：python tools/semantic_polish_batch.py
说明：这是临时批处理脚本，每个批次清空上批对照再填入新对照即可。
本次批次：_ 与 A 开头（_ / A / a）
"""

ROOT = Path(__file__).resolve().parents[1]
EN_DIR = ROOT / "English"

PAIR_RE = re.compile(r'^"(?P<lhs>(?:[^"\\]|\\.)*)"\s*=>\s*"(?P<rhs>(?:[^"\\]|\\.)*)"\s*$')

# 归一化：将左侧键和文件中的左侧文本都转成“ASCII + ?”形式，以兼容
# å/ä/ö 等变音在部分文件中被写成 '?' 的情况；仅用于匹配，不改变输出的原文。
def _norm_lhs(s: str) -> str:
  out = []
  for ch in s:
    # 保留 ASCII 可见字符与空格、标点；非 ASCII 或本就为 '?' 的统一成 '?'
    if ch == '?' or (not ch.isascii()):
      out.append('?')
    else:
      out.append(ch)
  return ''.join(out)


def replace_pairs(path: Path, mapping: dict[str, str]) -> int:
  changed = 0
  lines = path.read_text(encoding="utf-8").splitlines()
  out: list[str] = []
  # 预构建归一化后的映射
  norm_map: dict[str, str] = {_norm_lhs(k): v for k, v in mapping.items()}
  for line in lines:
    m = PAIR_RE.match(line)
    if not m:
      out.append(line)
      continue
    lhs = m.group('lhs')
    rhs = m.group('rhs')
    key_norm = _norm_lhs(lhs)
    if key_norm in norm_map:
      new_rhs = norm_map[key_norm].strip()
      if new_rhs.endswith('。'):
        new_rhs = new_rhs[:-1]
      if new_rhs != rhs:
        changed += 1
      out.append(f'"{lhs}" => "{new_rhs}"')
    else:
      out.append(line)
  if changed:
    path.write_text("\n".join(out) + "\n", encoding="utf-8")
  return changed


def main():
  total = 0

  # A-batch 映射：左侧以 A/a 开头的瑞典语句子
  a_batch_map: dict[str, str] = {
    # 常见感叹/寒暄
    'Adjö': 'Goodbye 再见',
    'Adjö då': 'Goodbye then 那再见',
    'Aha': 'Aha 啊哈',
    'Aha...': 'Aha... 啊哈……',
    'Aha!': 'Aha! 啊哈！',
    'Aha?': 'Aha? 啊哈？',
    'Ah...': 'Ah... 啊……',
    'Ah!': 'Ah! 啊！',
    'Ah': 'Ah 啊',
    'Ahhh!': 'Ahhh! 啊啊啊！',
    'Ah, tack!': 'Ah, thanks! 啊，谢谢！',
    'Ah, där har jag varit': "Ah, I've been there 嗯，我去过那儿",
    'Aj': 'Ouch 哎哟',

    # 指令/短句
    'Aktivera armen': 'Activate the arm 启用手臂',

    # 询问/对话套话
    'Allt väl?': 'Everything alright? 一切都好吗？',
    'Allting': 'Everything 一切',
    'Allting är sammankopplat': 'Everything is connected 万物相连',
    'Allt suger...': 'Everything sucks... 一切都糟透了……',
    'Alltid redo': 'Always ready 时刻准备着',
    'Allt som gör vardagen så praktisk!': 'Everything that makes everyday life so convenient! 让日常更方便的一切！',
    'Alltihop började när de började bryta malm i gruvan': 'It all started when they began mining ore in the mine 一切始于他们在矿井开始开采矿石的时候',

    # Alltså 开头口头禅类
    'Alltså...': 'I mean... 我是说……',
    'Alltså... typ': 'I mean... kinda 我是说……差不多吧',
    'Alltså, du går bara förbi färjan och svänger sedan höger runt huset': 'I mean, you just pass the ferry and then turn right around the house 你就从渡口那边走过去，然后绕着房子右转',
    'Alltså, du går över bron till Burrows och sen går du mot ministeriet': 'I mean, you cross the bridge to Burrows and then head towards the Ministry 你过桥去 Burrows，然后往部委那边走',
    'Alltså, du borde gå': 'I mean, you should go 我觉得你该走了',
    'Alltså hmm': 'I mean, hmm 我是说，嗯',
    'Alltså, hmm': 'I mean, hmm 我是说，嗯',
    'Alltså, jag vet inte': "I mean, I don't know 我也不太清楚",
    'Alltså, jag vet bara inte riktigt hur jag ska bete mig': "I mean, I just don't really know how to behave 我只是不太知道该怎么表现",
    'Alltså, jag kan inte så mycket om jazz': "I mean, I don't know much about jazz 我对爵士不太懂",
    'Alltså, jag tror inte det': "I mean, I don't think so 我觉得不是",
    'Alltså, jag tror att jag måste åka tillbaka...': 'I mean, I think I have to go back... 我想我得回去了……',
    'Alltså, jag tänkte bara säga...': 'I mean, I just wanted to say... 我就是想说……',
    'Alltså, lite': 'I mean, a little 有一点吧',
    'Alltså, jag har ett hotell bokat åt mig': 'I mean, I have a hotel booked for me 我这边已经订好酒店了',
    'Alltså, man måste försöka ha ett öppet sinne dude': 'I mean, you gotta try to keep an open mind, dude 我是说，得保持开放点，兄弟',
    'Alltså, om du kan hitta mer skit på honom så är det awesome': "I mean, if you can dig up more dirt on him, that'd be awesome 要是你能多挖到他点黑料就太棒了",
    'Alltså, Pixie jobbar ju inte med skor': "I mean, Pixie doesn't work with shoes Pixie 又不是干卖鞋的",
    'Alltså, principen för att ta bort spärrarna är mycket enkel': 'I mean, the principle for removing the restrictions is very simple 拆限制的原理其实很简单',
    'Alltså, spöken är inte så konstiga grejer som folk ofta får för sig': "I mean, ghosts aren't as weird as people think 我是说，鬼没大家想得那么离奇",
    'Alltså, vad gör ni där egentligen?': 'I mean, what are you actually doing there 你们到底在那干嘛？',
    'Alltså, vad kan man modifiera med den?': 'I mean, what can you modify with it 这个到底能改什么？',
    'Alltså, vi gömmer oss ju inte!': "I mean, we're not hiding! 我们又不是在躲着！",
    'Alltså... var ligger mitt rum? Jag hittar det inte': "I mean... where's my room? I can't find it 我房间在哪？我找不到",
    'Alltså, jag försöker komma på vem det är du liknar': "I mean, I'm trying to figure out who you look like 我在想你像谁",
    'Alltså det är ashäftigt att du vill vara med och hjälpa till här Sebbe': "I mean it's awesome that you want to help out here, Sebbe 你愿意来这帮忙真是太酷了，Sebbe",
    'Alltså det är lugnt Sebbe': "I mean it's fine, Sebbe 没事的，Sebbe",
    'Alltså det är något creepy med det här stället': "I mean there's something creepy about this place 我总觉得这地方有点渗人",
    'Alltså det är olika, jag fick mitt gig där av en slump': "I mean it depends, I got my gig there by chance 这要看情况，我那活儿是碰巧拿到的",
    'Alltså fan': 'I mean, damn 我靠',
    'Alltså hmm... finns det en chans för mig att få jobba här hos er?': 'I mean, hmm... is there a chance I could work here? 我在想……我有机会在这儿干活吗？',
    'Alltså snälla... kan jag inte få börja jobba hos er?': "I mean please... can't I start working with you? 拜托……我能不能在你们这儿干？",
    'Alltså vi trodde ju mest att han höll på att bli galen': "I mean we mostly thought he was going crazy 我们当时还以为他快疯了",
    'Alltså, ägaren brukar inte bli så glad': "I mean, the owner usually doesn't like that 老板一般不太高兴这个",
    'Alltså, den är väl inte jättebra egentligen': "I mean, it's not really that great 说实话，它也没多好",
    'Alltså, det är inte något allvarligt': "I mean, it's nothing serious 不是什么大事",
    'Alltså, det är ju flummiga grejer antar jag': "I mean, it's trippy stuff I guess 这玩意儿挺迷幻的，我猜",
    'Alltså, det är något med datorer va?': "I mean, it's something with computers, right? 跟电脑有关，对吧？",
    'Alltså, det är oftast gitarrer...': "I mean, it's usually guitars... 一般都是吉他……",
    'Alltså, det verkar inte finnas någon säng på mitt rum': "I mean, there doesn't seem to be a bed in my room 我房间里好像没有床",
    'Alltså, det verkar lite väl drastiskt kanske': "I mean, that seems a bit drastic maybe 这也太激进了点吧",
    'Alltså, du verkar ha gjort något alldeles amazing': "I mean, you seem to have done something absolutely amazing 你好像干了件真的很了不起的事",
    'Alltså, dude... du måste chilla': "I mean, dude... you gotta chill 兄弟……冷静点",
    'Alltså, helst inte': "I mean, preferably not 最好别",
    'Alltså, jag är inte så bra än': "I mean, I'm not that good yet 我现在还不太行",
    'Alltså, jag har ett antal fler samtal som jag måste ta här ikväll': "I mean, I have a bunch more calls to make tonight 我今晚还有一堆电话要打",

    # 其他 A 开头
    'Amääännnn...': 'Maaaan... 天哪……',
    'Aldrig hört namnet': 'Never heard the name 从没听说过这个名字',
    'Annars då?': 'How are things otherwise? 其他还好吗？',
    'Ah segt': 'Ah, bummer 哦，真糟',
    'Alright, bra': 'Alright, good 好的，行',
    'Andra våningen': 'Second floor 二楼',
    'Amen skaffa derå!': 'Then go get one! 那就搞一个啊！',

    # Araki 相关
    'Araki': 'Araki 荒木',
    'Araki är redan där inne tror jag': 'Araki is already in there, I think Araki 应该已经在里面了',
    "Araki pratar jämt om hur hon 'skapar liv'": "Araki keeps talking about how she 'creates life' Araki 老说她如何“创造生命”",
    'Araki!': 'Araki! 荒木！',

    # A 批内目标文件的通用句（提升语义与口径）
    # Albert_Ghosts
    'Var är jag?': 'Where am I? 我在哪？',
    'Var ÄR jag?!!?!': 'Where AM I?!!?! 我在哪？！?!',
    'Visst är det fantastiskt': "Isn't it fantastic? 这不是很棒吗？",
    'Det är vackert': "It's beautiful 真美",
    'Jag önskar att jag kunde ta in allt på samma gång': 'I wish I could take it all in at once 真希望我能一下子全都消化',
    'Det var tur, det finns så dåligt med sittplatser': "Lucky for us, there are so few seats around here 还好这样，这里座位实在太少了",
    'Varken Zarah eller jag kunde ju sova': "Neither Zarah nor I could sleep Zarah 和我都睡不着",
    'Zarah lyckades släpa in en soffa åt oss': 'Zarah managed to drag a sofa in here for us Zarah 设法给我们拖来了一张沙发',

    # 下划线场景常见修正
    'Jörgen, vi verkar ha fått besök under natten': 'Jörgen, looks like we had visitors last night Jörgen，看起来昨晚有人来过',
    'Jaså?': 'Oh, really? 哦，真的？',
    'Det stämmer!': "That's right! 没错！",
    'Oj, verkligen?!': 'Really?! 真的吗？！',
    'Gör inget motstånd': "Don't resist 不要反抗",
    'Var försiktig i så fall': 'In that case, be careful 那样的话小心点',
    'Mhm?': 'Mhm? 嗯？',
    'Mhm': 'Mhm 嗯',

    # Amanda_CanSellSoda
    'Svårt att fatta?': "Hard to understand? 很难懂吗？",
    'Hej, fortfarande inte intresserad': 'Hi, still not interested 我还是不感兴趣',

    # Araki_AfterGhostVisit（语气更口语化，术语更准确）
    'Tjena': 'Hey 嘿',
    'Läget?': "How's it going? 最近怎么样？",
    'Jag är seg i skallen': "My brain's sluggish 脑子有点转不动",
    'Läste en grym bok om funktionell komposition i går när jag skulle sova': "I read an awesome book on functional composition last night when I was supposed to sleep 我昨晚该睡的时候在看一本讲函数式组合的好书",
    'Abstrakt...': 'Abstract... 挺抽象的……',
    'Kleislipilar, om det säger dig något?': "Kleisli arrows, if that rings a bell? Kleisli 箭（范畴论）听起来熟吗？",
    'Japp, han är på sitt kontor': "Yeah, he's in his office 他在办公室",
    'Vilket är den största anledningen till att vi inte vet mer om deras organisation': "Which is the main reason we don't know more about their organization 这也是我们不了解他们组织更多内幕的主要原因",
  }

  # 仅对文件名以 '_' 或 A/a 开头的 .eng.mtf 应用本批映射（按文件批次）
  for path in EN_DIR.glob('*.eng.mtf'):
    name = path.name
    if not name:
      continue
    if name[0].lower() not in {'a', '_'}:
      continue
    total += replace_pairs(path, a_batch_map)

  print(f'Polished lines: {total}')


if __name__ == '__main__':
  main()

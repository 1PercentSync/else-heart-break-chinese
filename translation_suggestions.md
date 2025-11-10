# 游戏UI文本翻译建议与分析

这是在对 `elseMenuTrans/Assembly-CSharp` 目录下的C#反编译源码进行分析后，整理出的可翻译文本、翻译建议及分析报告。

---

## 第一批翻译对照表

| 原始文本 | 翻译建议 | 位置提示 (文件名) |
| :--- | :--- | :--- |
| `pick up` | `pick up` 捡起 | `PlayerRoamingState.cs` |
| `Start computer (no monitor)` | `Start computer (no monitor)` 启动电脑 (无显示器) | `PlayerRoamingState.cs` |
| `hack` | `hack` 黑入 | `PlayerRoamingState.cs` |
| `open bag` | `open bag` 打开背包 | `PlayerRoamingState.cs` |
| `open` | `open` 打开 | `PlayerRoamingState.cs` |
| `dance` | `dance` 跳舞 | `PlayerRoamingState.cs` |
| `give` | `give` 给予 | `PlayerRoamingState.cs` |
| `put down` | `put down` 放下 | `PlayerRoamingState.cs` |
| `put into bag` | `put into bag` 放入背包 | `PlayerRoamingState.cs` |
| `read` | `read` 阅读 | `PlayerRoamingState.cs` |
| `close bag` | `close bag` 关闭背包 | `PlayerRoamingState.cs` |
| `Too tired to run` | `Too tired to run` 太累了，跑不动 | `PlayerRoamingState.cs` |
| `Reality blocked, can't jack out!` | `Reality blocked, can't jack out!` 现实被阻断，无法拔出！ | `InsideComputerState.cs` |
| `Available connections:` | `Available connections:` 可用连接: | `InsideComputerState.cs` |
| `INVALID TARGET` | `INVALID TARGET` 无效目标 | `InsideComputerState.cs` |
| `move to` | `move to` 移动至 | `InsideComputerState.cs` |
| `exit` | `exit` 退出 | `InsideComputerState.cs` |
| `ATTACKED` | `ATTACKED` 受到攻击 | `InsideComputerState.cs` |
| `LOADING` | `LOADING` 正在加载 | `InsideComputerState.cs` |
| `Command:` | `Command:` 命令: | `CommandLine.cs` |
| `Run` | `Run` 运行 | `CommandLine.cs` |

### 分析:
* `PlayerRoamingState.cs` 是一个宝库，包含了大量核心的交互文本，如“捡起”、“放下”、“打开背包”等。
* `InsideComputerState.cs` 包含了“骇入”网络后的界面文本。
* `CameraFocusPoint.cs` 中包含了一个巨大的、由几百个英文人名组成的数组，这些似乎是用于随机生成的，我判断它们不需要翻译，因此没有加入表格。
* `CharacterShell.cs` 包含大量文件名和内部标识符，这些都不能翻译，我已经将其过滤。

---

## 第二批翻译对照表

| 原始文本 | 翻译建议 | 位置提示 (文件名) |
| :--- | :--- | :--- |
| `Error:` | `Error:` 错误: | `CodeEditorSuggestionMaker.cs` |
| `Error on line` | `Error on line` 行错误 | `CodeEditorSuggestionMaker.cs` |
| `Suggestions:` | `Suggestions:` 建议: | `CodeEditorSuggestionMaker.cs` |
| `Can't load texture '` | `Can't load texture '` 无法加载纹理 ' | `Container.cs` |
| `Couldn't find child` | `Couldn't find child` 未找到子对象 | `Container.cs` |
| `in container` | `in container` 于容器 | `Container.cs` |
| `Command:` | `Command:` 命令: | `CommandLine.cs` |
| `Run` | `Run` 运行 | `CommandLine.cs` |
| `was executed successfully` | `was executed successfully` 已成功执行 | `CommandLine.cs` |
| `Error in command:` | `Error in command:` 命令错误: | `CommandLine.cs` |
| `(autosave)` | `(autosave)` (自动存档) | `FastForwardState.cs` |
| `Will not show cheats!` | `Will not show cheats!` 不会显示作弊选项！ | `DialogueSubstate.cs` |
| `Got DEFOCUS message but is not focused on '` | `Got DEFOCUS message but is not focused on '` 收到了散焦消息但并未聚焦于 ' | `DialogueSubstate.cs` |
| `', so will not end dialogue state` | `', so will not end dialogue state` '，因此不会结束对话状态 | `DialogueSubstate.cs` |
| `nextNodes.Length == 0 in dialogue` | `nextNodes.Length == 0 in dialogue` 对话中 nextNodes.Length == 0 | `DialogueSubstate.cs` |
| `Skipping cheating option node:` | `Skipping cheating option node:` 跳过作弊选项节点: | `DialogueSubstate.cs` |
| `Branch node is null!` | `Branch node is null!` 分支节点为空！ | `DialogueSubstate.cs` |
| `Can't choose option` | `Can't choose option` 无法选择选项 | `DialogueSubstate.cs` |
| `, there are only` | `, there are only` , 只有 | `DialogueSubstate.cs` |
| `option nodes.` | `option nodes.` 个选项节点。 | `DialogueSubstate.cs` |

### 分析:
* `DialogueSubstate.cs` 包含了对话系统中的一些调试和错误信息，这些在测试时可能会看到。
* `CodeEditorSuggestionMaker.cs` 包含了游戏内代码编辑器的错误和建议提示。
* `FastForwardState.cs` 包含了快进（如睡觉）时的自动存档文本。
* `Container.cs` 和 `CommandLine.cs` 包含了一些通用的UI和调试信息。

---

## 第三批翻译对照表

| 原始文本 | 翻译建议 | 位置提示 (文件名) |
| :--- | :--- | :--- |
| `Will change back to character avatar's room:` | `Will change back to character avatar's room:` 将切换回角色化身的房间: | `TramRideState.cs` |
| `Somewhere` | `Somewhere` 某个地方 | `Sign.cs` |

### 分析:
* `TramRideState.cs` 包含乘坐电车时的状态切换信息。
* `Sign.cs` 是一个简单的路牌对象，它有一个默认文本 "Somewhere"。
* 其他文件如 `ButtonShell.cs`, `SeatShell.cs`, `TramShell.cs` 等，主要是物件的行为逻辑，虽然它们加载了声音资源，但没有面向用户的可翻译文本。

---

## 第四批翻译对照表

| 原始文本 | 翻译建议 | 位置提示 (文件名) |
| :--- | :--- | :--- |
| `Looking inside locker` | `Looking inside locker` 正在查看储物柜 | `InsideLockerState.cs` |
| `EXIT` | `EXIT` 退出 | `InsideLockerState.cs` |
| `NEXT` | `NEXT` 下一个 | `InsideLockerState.cs` |
| `TAKE ITEM` | `TAKE ITEM` 拿取物品 | `InsideLockerState.cs` |
| `Set game volume from player prefs to` | `Set game volume from player prefs to` 从玩家偏好设置游戏音量为 | `MainMenu.cs` |
| `Got resolution setting from player prefs:` | `Got resolution setting from player prefs:` 从玩家偏好中获取分辨率设置: | `MainMenu.cs` |
| `Got fullscreen setting from player prefs:` | `Got fullscreen setting from player prefs:` 从玩家偏好中获取全屏设置: | `MainMenu.cs` |
| `Setting resolution to` | `Setting resolution to` 设置分辨率为 | `MainMenu.cs` |
| `NOT FULLSCREEN` | `NOT FULLSCREEN` 非全屏 | `MainMenu.cs` |
| `FULLSCREEN` | `FULLSCREEN` 全屏 | `MainMenu.cs` |
| `Got Quality setting from player prefs:` | `Got Quality setting from player prefs:` 从玩家偏好中获取画质设置: | `MainMenu.cs` |
| `Got no Quality setting from player prefs, using 3 (middle)` | `Got no Quality setting from player prefs, using 3 (middle)` 未从玩家偏好中获取画质设置，使用 3 (中等) | `MainMenu.cs` |
| `Got AdvancedShaders setting from player prefs:` | `Got AdvancedShaders setting from player prefs:` 从玩家偏好中获取高级着色器设置: | `MainMenu.cs` |
| `Got InvertCamera setting from player prefs:` | `Got InvertCamera setting from player prefs:` 从玩家偏好中获取反转镜头设置: | `MainMenu.cs` |
| `Got TextSpeed setting from player prefs:` | `Got TextSpeed setting from player prefs:` 从玩家偏好中获取文本速度设置: | `MainMenu.cs` |
| `Got AutoZoom setting from player prefs:` | `Got AutoZoom setting from player prefs:` 从玩家偏好中获取自动缩放设置: | `MainMenu.cs` |
| `Resume` | `Resume` 继续 | `PauseMenu.cs` |
| `Save` | `Save` 保存 | `PauseMenu.cs` |
| `Refreshing Options Panel` | `Refreshing Options Panel` 正在刷新选项面板 | `OptionsPanel.cs` |
| `Options panel, shaders:` | `Options panel, shaders:` 选项面板, 着色器: | `OptionsPanel.cs` |
| `volume:` | `volume:` 音量: | `OptionsPanel.cs` |
| `Creating resolution button for resolution` | `Creating resolution button for resolution` 为分辨率创建分辨率按钮 | `OptionsPanel.cs` |
| `Native (` | `Native (` 原生 ( | `OptionsPanel.cs` |
| `Wrote to res_width, res_height player prefs:` | `Wrote to res_width, res_height player prefs:` 已将 res_width, res_height 写入玩家偏好: | `OptionsPanel.cs` |
| `Wrote quality to player prefs:` | `Wrote quality to player prefs:` 已将画质写入玩家偏好: | `OptionsPanel.cs` |
| `Wrote to AutoZoom player prefs:` | `Wrote to AutoZoom player prefs:` 已将自动缩放写入玩家偏好: | `OptionsPanel.cs` |
| `Wrote to AdvancedShaders player prefs:` | `Wrote to AdvancedShaders player prefs:` 已将高级着色器写入玩家偏好: | `OptionsPanel.cs` |
| `Wrote to InvertCamera player prefs:` | `Wrote to InvertCamera player prefs:` 已将反转镜头写入玩家偏好: | `OptionsPanel.cs` |
| `Saved MasterVolume:` | `Saved MasterVolume:` 已保存主音量: | `OptionsPanel.cs` |
| `Saved TextSpeed:` | `Saved TextSpeed:` 已保存文本速度: | `OptionsPanel.cs` |

### 分析:
* `MainMenu.cs` 和 `OptionsPanel.cs` 包含了大量的设置和调试信息，这些信息在游戏启动和配置时非常重要。
* `PauseMenu.cs` 和 `PlayerPauseMenu.cs` 提供了暂停菜单中的“继续”和“保存”等功能文本。
* `InsideLockerState.cs` 包含了查看储物柜时的UI文本。

---

## 第五批翻译对照表

| 原始文本 | 翻译建议 | 位置提示 (文件名) |
| :--- | :--- | :--- |
| `No AttackEffect found on` | `No AttackEffect found on` 未找到攻击效果于 | `InternetSentry.cs` |

### 分析:
* 这批文件非常“干净”，几乎没有需要翻译的字符串。这表明我们正在处理游戏的核心后台逻辑部分。
* `HackdevScreen.cs` 和 `Hackdev.cs` 虽然听起来像是会有UI文本的地方，但它们似乎只处理逻辑，而将渲染和文本显示交给了其他模块（比如我们之前分析过的 `CodeEditor...` 文件）。

---

## 后续批次分析总结

### 第六批
第六批20个文件（主要位于 `GameWorld` 和 `GameTypes` 目录下）已经处理完毕。这些文件是游戏世界的核心数据结构和逻辑，例如寻路（`Pathfinder.cs`）、时间系统（`GameTime.cs`）和对话流程（`DialogueRunner.cs`）。经过分析，这批文件中不包含任何面向用户的可翻译文本。

### 第七批
第七批20个文件（主要位于 `GameTypes/RelayLib` 和 `GameTypes/ProgrammingLanguageNr1` 目录下）已经处理完毕。这些文件定义了游戏的数据序列化库和内置编程语言的结构（如解析器、语法节点等）。经过分析，这批文件中同样不包含任何面向用户的可翻译文本。

### 第八批
第八批20个文件（主要位于 `GameTypes/ProgrammingLanguageNr1` 和 `GameTypes/GrimmLib` 目录下）已经处理完毕。这些文件继续深入游戏的核心引擎，定义了游戏内编程语言的错误处理、作用域、表达式以及对话系统的节点、分支和API。经过分析，这批文件中依然不包含任何面向用户的可翻译文本。

### 第九批
第九批20个文件（主要位于 `GameTypes` 和 `GameWorld2` 目录下）已经处理完毕。这些文件定义了游戏中的各种实体和概念，例如手提箱(`Suitcase.cs`)、电车(`Tram.cs`)、电话(`Telephone.cs`)以及世界设定(`WorldSettings.cs`)。经过分析，这些文件负责对象的行为逻辑，但不包含任何面向用户的可翻译文本。

### 最终批
最后一批21个文件（位于 `GameWorld2` 目录下）已经处理完毕。这批文件同样是游戏核心对象的逻辑定义，例如角色(`Character.cs`)、电脑(`Computer.cs`)、门(`Door.cs`)等。与预期一致，这些文件负责定义对象的属性和行为，不包含任何面向用户的可翻译文本。
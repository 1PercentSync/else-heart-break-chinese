商店页面
Else Heart.Break()
全部 讨论 截图 艺术作品 实况直播 视频 新闻 
指南
 评测
Else Heart.Break() > 指南 > hyphz 的指南

84 个评价

Full SPRAK Command Reference
由 hyphz 制作
List of all SPRAK commands on all computers.
 3
 2




 
 
奖励
收藏
分享
创建者

hyphz
离线
类型: 制作, 秘密, 攻略
Languages: 英语
发表于
2016 年 1 月 31 日 上午 6:57
4,215	不重复访客数
156	当前收藏人数
指南索引

总览
A 
B 
C 
D 
E 
F 
G 
H 
I 
L 
M 
N 
P 
Q 
R 
S 
T 
U 
W 
Z 
Notes 
留言
A
Prototype	Function	API	Connection
Append(*array, item)	Appends item to array.
B
Prototype	Function	API	Connection
void BootFromFloppy()	Runs program from inserted floppy
void Break()	Story trigger	Heart	Heart
Broadcast
C
Prototype	Function	API	Connection
void Charisma(int)	Modifies user's charisma	Consumables
int CharToInt(string)	Converts a string representation of a number to that number
ClearData	Clears data on a floppy disc	Floppy
ClearMemories	Clears internal hashtable storage
ClearText	Clears textual display
Color
connection Connect(string)	Creates connection to named computer	internet
Cos	Calculates cosine of a number
void CopyToClipboard(string)	Put the named string onto the clipboard (the real Windows one!)
int Count(array)	Returns the length of the array.
D
Prototype	Function	API	Connection
void DisconnectAll()	Breaks all connections
void DisplayGraphics()	Clears screen and runs graphics queue
E
Prototype	Function	API	Connection
void EnableAPI(string)	Activate named API on target computer	Screwdrivers
EraseMemory	Delete a key from internal hashtable storage
F
Prototype	Function	API	Connection
void FastForward()	Trigger time speed up effect	Drugs
array FindPath(string, string)	Returns a list of room identifiers giving the path between objects	
G
Prototype	Function	API	Connection
GetAction	Gets the action named NPC is currently performing	MainFrame	MainFrame
array GetAllRooms()	Gets the entity names of every room in the game	MainFrame	MainFrame
GetCharisma	Gets the Charisma score of named NPC	MainFrame	MainFrame
GetConnections	Get the list of connected other computers
GetIndexes
string GetLabel()	Get the user-set label for the target entity	Extractors
GetMemories
string GetName()	Gets name of activated item	Extractors
string GetNameOfCardOwner()	Gets the name of the card owner	Credit cards
int GetNumericData(string, string)	Gets named numeric parameter for an object/NPC	Heart	Heart
array GetPeople()	Gets the entity names of every person in the game, including Sebastian	MainFrame	MainFrame
string GetPosition(string)	Gets the room name and coordinates of named entity.	MainFrame	MainFrame
string GetRoom(string)	Gets the room where the named entity is	MainFrame	MainFrame
int GetSleepiness(string)	Gets person's sleepiness level	MainFrame	MainFrame
GetSpeed	Gets a named person's movement speed	MainFrame	MainFrame
array GetThingsInRoom(string)	Gets list of entity names of everything in specified room	MainFrame	MainFrame
array GetThingsOfType(string)	Gets list of entity names of everything of specified type	MainFrame	MainFrame
int GetHour()	Gets current hour
int GetMinute()	Gets current minute
int GetRain()	Get level of rain	Meteorology	MeteorologyServer
string GetUser()	Returns name of current user
void Goto(string)	Moves user to named object	Doors
H
Prototype	Function	API	Connection
HasFloppy	Returns if machine implements the floppy API
boolean HasFunction ( string )	Undocumented! Returns True if named function is available
HasIndex
HasMemory	Returns true if internal hashtable contains named key
HD
int Height()	Get screen height
HSVtoRGB	Converts colour in Hue/Saturation/Value format to Red/Green/Blue
I
Prototype	Function	API	Connection
void Info()	Prints out computer's name, speed, APIs and screen size
Input	Displays the given string and asks the user for string input
Int	Converts a float or string value into an integer
InteractWith(string, string)	Puts second named item into first's interaction queue	MainFrame	MainFrame
string IntToChar(int)	Returns the character with the given index in the character set
boolean IsKeyPressed(string)	Check if a "game control" key is pressed
L
Prototype	Function	API	Connection
Line	Add a line to the graphics queue	Graphics
Lines	Add multiple lines to the graphics queue	Graphics
LoadData	Load a string from an inserted floppy disc	Floppy
LoadMemory
void Lock(String)	Lock any door by name	Hugin	Hugin
bool Lock(int)	Lock target door with a code number	Keys
M
Prototype	Function	API	Connection
Mod	Calculates an integer modulus
MovePerson
N
Prototype	Function	API	Connection
string Name()	Get name of the computer
P
Prototype	Function	API	Connection
Pitch
Position
PlaySound
void Print(string)	Print string on the text display
void PrintS(string)	Print string on the text display without ending the line
Q
Prototype	Function	API	Connection
QuickBoost	Applies a temporary speed boost	Drugs
Quit
R
Prototype	Function	API	Connection
Random	Calculates a random number
array Range(int, int)	Return an array of all counting numbers between the two inputs
Rect	Adds a rectangle to the graphics queue
int Repeat(int)	Equivalent to Mod with a less scary mathematical name
RemoteFunctionCall(connection, string, array)	Call a remote function giving name as a string rather than wired into the program
Remove
RemoveAll
RestoreCode	Reverts the code in named computer back to the game default	Traps	FactoryLobbyTrap
RGBtoHSV	Converts colors in red/green/blue format to hue/saturation/value[/tr]
Round
[/table]
S
Prototype	Function	API	Connection
SaveData
SaveMemory
Say	Display string on textual monitor if in view, or in a top-up if not
void SetMaxTime(int)	Sets maximum execution time for a computer	Screwdrivers
void SetMhz(int)	Sets speed of a computer	Screwdrivers
void SetNumericData(string, string, int)	Set any named numeric field on the named entity	Heart	Heart
void SetRain(int)	Set the current level of rain in the world	Meteorology	MeteorologyServer
void SetLabel(string)	Set the user label of the target entity	Extractors
void SetWorldPosition(string, int, int)	Move active user to given coordinates in named room	Teleporters
void SetPosition(string, string)	Move first object to location of second object - can't move Sebastian	MainFrame	MainFrame
Sin	Calculates sin of a number
StringContains	Tests if a string is a substring of another
void Sleep(int)	Stall for specified number of seconds
void Sleepiness(int)	Modifies user's sleepiness	Drinks
void Slurp()	Trigger cyberspace view
void Switch(string)	Toggle the specified lamp	PowerTap	PowerTap
T
Prototype	Function	API	Connection
void Teleport(int, int)	Move active user to given coordinates in the same room
void Text(string, int, int)	Add text at given coordinate to graphics queue
bool Toggle(int)	Toggle target door with a code number	Keys
float Time()	Get Julian time as a float
Trippy	Trigger trippy graphical effects	Drugs
TurnLeft	Turn moving object left	Turtles
TurnRight	Turn moving object right	Turtles
void TurnOff(string)	Turn off a specified lamp	PowerTap	PowerTap
void TurnOn(string)	Turn on a specified lamp	PowerTap	PowerTap
string Type()	Get string giving datatype of the argument (not same as object type)
U
Prototype	Function	API	Connection
void Unlock(String)	Unlock any door by name	Hugin	Hugin
bool Unlock(int)	Unlock target door with a code number	Keys
W
Prototype	Function	API	Connection
int Width()	Get screen width
Z
Prototype	Function	API	Connection
void ZapPerson(String name)	Shocks the target person in the same room, and knocks them out (Sebastian respawns)	Traps	FactoryLobbyTrap
void ZapPersonGently(String name)	Shocks the target person in the same room but doesn't knock them out	Heart	Heart
Notes
This guide lists only SPRAK primitives. Functions like GetBalance on the FinanceServer are not listed because they are defined functions, not primitives.
Functions that refer to a "user" or "target" can't be used by remote connections; they must be called in the context of the object having been used by a character (probably Sebastian)
A teleportation device is available in the poor area from the start of the game; once you have this and a basic modifier you can go anywhere
6 条留言
 订阅讨论串
(?)

添加一条留言

Fruity Trump 2023 年 12 月 16 日 上午 10:25 
This is insane！！！！:APTrose:

瞻云就日 2023 年 10 月 31 日 下午 8:51 
感谢感谢

Llamamoe 2017 年 9 月 6 日 下午 2:20 
This guide does not include GetTypeOfThing, possibly other stuff(added by patches?)

Sorrien 2017 年 1 月 27 日 上午 6:05 
Tram commands?

Geeky Meerkat 2016 年 12 月 9 日 上午 5:10 
commands like GetSleepiness don't seem to be part of MainFrame but instead the extractor, and the extractor doesn't take a string input.

9214 2016 年 3 月 3 日 下午 4:49 
ZapPersonGently() works even if person isn't in the same room with Heart btw.
Valve 徽标
© Valve Corporation。保留所有权利。所有商标均为其在美国及其它国家/地区的各自持有者所有。 本网站上部分地理空间数据由 geonames.org 提供。
隐私政策   |  法律信息   |  无障碍  |  Steam 订户协议  |  Cookie
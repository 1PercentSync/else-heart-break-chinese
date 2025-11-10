Steam 主页链接商店社区关于客服安装 Steam 登录  |  语言
商店页面
Else Heart.Break()
全部 讨论 截图 艺术作品 实况直播 视频 新闻 
指南
 评测
Else Heart.Break() > 指南 > Balthasar 的指南
Else Heart.Break() > 指南 > 巴尔萨泽的指南

55 个评价

Coding Guide - Not only first steps
编程指南——不止是入门教程
由 Balthasar 制作
The aim of this guide is to show you some of the possibilities, the game provides. Also it covers some basic coding issues.
本指南旨在展示游戏提供的部分可能性，同时涵盖一些基础编程问题。

I don't want to start from scratch but i'll still try to explain every step in detail.
虽然不想从零开始讲解，但我仍会尝试详细解释每个步骤。

 2
 
 
奖励
收藏
分享
创建者

Balthasar
巴尔萨泽

离线
类型: 秘密, 攻略
Languages: 英语
语言：英语
发表于
更新日期
2015 年 10 月 4 日 上午 4:07
2015 年 10 月 10 日 上午 2:26
3,550	不重复访客数
98	当前收藏人数
指南索引
总览
Necessary Tools
必备工具 
Part I - Connect() and Slurp()
第一部分 - Connect() 与 Slurp() 函数 
Part II - Creating a Main Menu
第二部分 - 创建主菜单 
Part III - Calling (an)other function(s) and dealing with objects
第三部分 - 调用其他函数与对象处理 

Part IV - Passing parameters to other functions
第四部分 - 向其他函数传递参数 
Part V - Remote Connections / Unlocking Doors I
第五部分 - 远程连接/解锁门禁 I 
Part V - Remote Connections / Unlocking Doors II
第五部分 - 远程连接/解锁门禁 II 
留言
Necessary Tools
必备工具
You'll need a modifier of course. If you don't own one yet look into my guide for lockpicking to get one.
你当然需要一个修改器。如果还没有的话，可以参考我的开锁指南获取一个。

Another helpful tool is the teleporting device but it's not necessary for our case.
另一个有用的工具是传送装置，但在我们这种情况下并非必需。

The most important thing is to find some computer or object that has all functions you want to use. Some basic interface like the computer at the reception of your hotel should be ok for now.
最重要的是找到具备所有所需功能的电脑或对象。像酒店前台那种基础界面的电脑目前就够用了。



Just uncommend the existing code with "#" in front or delete it completely.
只需取消以"#"开头的现有代码注释，或将其完全删除。


Part I - Connect() and Slurp()
第一部分 - Connect() 与 Slurp() 函数
Ok, now that you hopefully found some computer, that provides you with some basic functions (including the Connect() and the Slurp() function) let's go straight into the code:
好了，既然你已经找到了具备基础功能（包括 Connect() 和 Slurp() 函数）的电脑，让我们直接进入代码部分：


Every computer, that has Connect and Slurp can basically get you anywhere you want in the game.
任何拥有 Connect 和 Slurp 功能的电脑，基本上都能带你去往游戏中的任何地方。


For now, we want to connect to some other place and go there.
目前，我们想要连接到某个其他地方并前往那里。

The function to connect to some other object is "Connect()" , obviously :).
连接到其他对象的函数显然是"Connect()"。

But it needs the right argument. It needs the exact name (the exact string) of it. For example, we want to move back to our little room. Type in:
但它需要正确的参数。它需要确切的名称（确切的字符串）。例如，我们想要回到我们的小房间。请输入：

Connect("Hotel_Room1_Door1")

This will connect us with our door basically. The problem here is to get the names of all the objects. There is a device you'll find later on, that extracts the name of any object you use it on but for now you will just have to trust me on this :).
这样我们基本上就能连接到门了。现在的问题是要获取所有对象的名称。稍后你会找到一个设备，可以对任何对象使用并提取其名称，但现在你只能先相信我的话了 :)

Ok, the program connected to your door. Next step is to get there.
好的，程序已连接到你的门。下一步是到达那里。

So, we need the Slurp() function:
所以，我们需要用到 Slurp() 函数：

Type in:
请输入：
Slurp()
吸溜()



If you now use the computer you will be slurped into the internet. Choose the target (in this case it should be Hotel_Room1_Door1, to which we connected before) and press "q" for exit when you get there.
如果你现在使用这台电脑，你将被吸入互联网。选择目标（此处应为之前连接过的 Hotel_Room1_Door1），到达后按"q"键退出。


Now you should be in your room again.
现在你应该已经回到自己的房间了。


Note: Many non-computer-objects own a Connect() function but only a few types of them also have the Slurp() function.
注意：许多非计算机对象都拥有 Connect() 功能，但其中只有少数类型同时具备 Slurp() 功能。
Part II - Creating a Main Menu
第二部分 - 创建主菜单
Let's say you want to connect and travel (Slurp) to some other object somewhere else and you want to find out if the object you are traveling to has a certain function.
假设你想要连接并传送（啜饮）到其他地方的某个对象，同时想确认目标对象是否具备特定功能。
For example you want to know if an object you are slurping to has also a Slurp function, to get you back into the internet.
例如你想知道通过啜饮抵达的对象是否也具备啜饮功能，以便让你返回互联网。

Well, usually an object you can travel to directly also has this function but let's assume you don't know or simply want to check, if the object provides some other function you may miss on the one you are at the moment.
通常可直接传送的对象都会具备此功能，但假设你并不确定，或只是想确认该对象是否提供当前所处对象所缺失的其他功能。

First we will write a start to write a small menue, so that the user has some option(s) to choose from. For now this will only include the search for a function of an object but we may want to expand this later on.
首先，我们将编写一个简单的菜单，以便用户有一些选项可供选择。目前这仅包括搜索对象的功能，但我们以后可能会扩展它。

The Main Menu
主菜单

First search for a computer with a bigger screen. A laptop or maybe the one at Wellspring Soda building should do.
首先寻找一台屏幕更大的电脑。笔记本电脑或泉水苏打大楼里的那台应该可以。

Instead of just typing in some functions we now will start to write an own function, which will be the main entry point. So...maybe we call it main() or to make it more obvious in this case MainMenu():
现在我们将不再只是输入一些函数，而是开始编写一个自己的函数，它将作为主要入口点。所以……也许我们称它为 main()，或者为了更明确起见，称它为 MainMenu()：

void MainMenu()
   #some code is missing here
end
void MainMenu()
   #此处缺失部分代码
end

void is the parameter, that tells the compiler, that this is a function. The brackets behind "MainMenue" are mandatory. Now we have this nice little function but at the moment it does...nothing :). Also, it won't yet be called by the computer, when you run it.
void 是告知编译器这是一个函数的参数。"MainMenu"后面的括号是必需的。现在我们有了这个不错的小函数，但目前它...什么都不做：）。此外，当你运行它时，计算机还不会调用这个函数。
So let's fix this and throw in some more code:
那么我们来修复这个问题，并加入更多代码：

MainMenu()

void MainMenu()
   ClearText()
   Print("Main Menu")
   Print("=========")
   Print("")
   Print("1. Check for function")
   Print("")
   Print("Please choose an option.")
   var mChoice = Input("")
end
主菜单()

void 主菜单()
   清空文本()
打印("主菜单")
打印("=========")
打印("")
打印("1. 检查功能")
打印("")
打印("请选择一个选项。")
var mChoice = 输入("")
结束

Now we first call our function MainMenu() at the start. When it's called the screen gets cleared via ClearText(). For now you should have an empty screen anyway but it will be important for later, when you jump back to the main entry point of your program.
现在我们先在程序开始时调用主菜单函数。调用时屏幕会通过清屏函数被清除。虽然目前你的屏幕应该是空白的，但之后当你跳转回程序主入口点时，这个操作就会显得很重要。

The Print-command should be self-explaining. Below this we actually do two things:
打印命令的含义应该不言自明。在此之下我们实际上执行了两个操作：

var mChoice
declares a variable mChoice (the m just stands for main in this case) and
声明了一个变量 mChoice（此处的 m 仅代表主菜单之意）

Input("")
输入("")
initialises the variable. In this case the variable isn't directly initialised with a certain value but with another function Input(""), that awaits the users input on the screen, which is then saved within var mChoice.
初始化变量。在这种情况下，变量并非直接赋予特定值，而是通过另一个函数输入("")进行初始化，该函数会等待用户在屏幕上输入内容，随后将输入值保存在变量 mChoice 中。

You also could write:
你也可以这样写：

var mChoise = Input("Please choose an option.")
var mChoice = 输入("请选择一个选项。")
It does the same but this case the users input would be followed directly after the line instead one line below.
它的作用相同，但这次用户的输入会直接跟在当前行后面，而不是换到下一行。

Ok, so now the user typed in something. He doesn't have much choices either at the moment :). So either he types "1" or something else. Whatever he typed we store it into mChoice and now will have to do something with this:
好的，现在用户输入了一些内容。目前他也没有太多选择：）。所以他要么输入"1"，要么输入其他内容。无论他输入什么，我们都会将其存储到 mChoice 中，现在需要对此进行处理：

MainMenu()

void MainMenu()
   ClearText()
   Print("Main Menu")
   Print("=========")
   Print("")
   Print("1. Check for function")
   Print("")
   Print("Please choose an option.
MainMenu()

void MainMenu()
清空文本()
打印("主菜单")
打印("=========")
打印("")
打印("1. 检查函数功能")
打印("")
打印("请选择一个选项。
")
   #saves users input into mChoice
   var mChoice = Input("")
  
   if(mChoice == "1")
       Print("Excellent choice!")
       #do something
   else
       Print("I'm sorry but i didn't understand " + mChoice)
       Input("Please press the 'any key'.
")
# 将用户输入保存至 mChoice 变量
   var mChoice = Input("")

   if(mChoice == "1")
       Print("明智的选择！")
#执行某些操作
   否则
       打印("抱歉，我没能理解 " + mChoice)
       输入("请按'任意键'")
")
       MainMenu()
   end 
end
")
       主菜单()
   结束
结束

Ok, what do we have here?
好的，我们来看看这里有什么？

if(mChoice == "1")
       Print("Excellent choice!")
       #do something
if(mChoice == "1")
       打印("绝佳的选择！")
       #执行某些操作

With the if command we check if the user typed in "1". If this turns out to be true the program goes on and just prints some text.
通过 if 命令我们检查用户是否输入了"1"。若结果为真，程序将继续执行并仅输出一些文本。

else
       Print("I'm sorry but i didn't understand " + mChoice)
       Input("Please press the 'any key'.")
       MainMenu()
   end
否则
       打印("很抱歉，我未能理解 " + 用户选择)
       输入("请按'任意键'。")
主菜单()
结束

If the condition is untrue (false) the program will jump to this point and aks the user to press a key. After this we call our MainMenu() function again and the program will start again from the beginning.
若条件不成立（为假），程序将跳转至此处并提示用户按下任意键。随后我们将再次调用主菜单函数，程序便会从头重新开始运行。

Note, that in this case we have the Input() command again but this time it just waits for the users input.
请注意，此处虽然再次出现了输入指令，但这次仅仅是等待用户进行输入操作。

Let's assume the user finally decides to choose option 1 (after a long time he carefully thought about his options :) ). He wants to check if some object has a certain function and thus can handle some specific task for him.
假设用户最终决定选择选项 1（在他长时间仔细考虑过所有选项之后 :)）。他想检查某个对象是否具有特定功能，从而能为他处理某些特定任务。
Now we first need him to tell us the name of the object he wants to check.
现在我们需要他先告诉我们想要检查的对象名称。
We could go on and directly ask the user for another input within the if/else - check but to have a better structure of our little program we will call another function from there:
我们可以继续在 if/else 判断中直接要求用户提供另一个输入，但为了让我们这个小程序结构更清晰，我们将从那里调用另一个函数：

if(mChoice == "1")
       Print("Excellent choice!")
       CheckObject()
if(mChoice == "1")
打印("绝佳选择！")
检查对象()

Of course we will have to create this function too, to make it accessable:
当然我们也需要创建这个函数，使其可被调用：

void CheckObject()
   #some awsome code in here
end
void 检查对象()
#这里有一些超棒的代码
结束

You can write this at any place within the program but for the sake of readability we will create this function below our MainMenu().
你可以在程序中的任意位置编写这段代码，但为了代码可读性，我们将在 MainMenu()函数下方创建这个函数。


Part III - Calling (an)other function(s) and dealing with objects
第三部分 - 调用其他函数及对象处理
This is what we have so far:
目前我们已完成的内容如下：

MainMenu()

void MainMenu()
   ClearText()
   Print("Main Menu")
   Print("=========")
   Print("")
   Print("1. Check for function")
   Print("")
   Print("Please choose an option.
主菜单()

void 主菜单()
   清空文本()
打印("主菜单")
打印("=========")
打印("")
打印("1. 检查功能")
打印("")
打印("请选择一个选项。
")
   #saves users input into mChoice
   var mChoice = Input("")
  
   if(mChoice == "1")
       Print("Excellent choice!")
      CheckObject()
   else
       Print("I'm sorry but i didn't understand " + mChoice)
       Input("Please press the 'any key'.
")
#将用户输入保存到 mChoice 变量中
var mChoice = Input("")

   if(mChoice == "1")
       Print("绝佳选择！")
      CheckObject()
否则
打印("抱歉，我没有理解 " + mChoice)
请按“任意键”。
")
       MainMenu()
   end 
end

void CheckObject()
   #some awsome code in here
end
")
主菜单()
结束
结束

void 检查对象()
#这里有些超棒的代码
结束

When CheckObject() is called we want the user to specify the name of it and which function he searchs for:
当调用 CheckObject()时，我们希望用户指定对象名称及其要查找的功能：

void CheckObject()
   #clear screen again
   ClearText()
   Print("Please type in the name of the object.")
   #save users input in var oName
   var oName = Input("")
   ClearText()
   
   Print("Which function you search for?
void CheckObject()
再次清屏
ClearText()
Print("请输入对象名称。")
将用户输入保存至变量 oName
var oName = Input("")
   ClearText()

   Print("您要搜索哪个功能？
")
   var oFunc = Input("")
    
   #connect to the object
   var cName = Connect(oName)
   
   #check if object has the function, the user searches for
   if(cName.
")
var oFunc = Input("")

   #连接到对象
   var cName = Connect(oName)

   #检查对象是否包含用户查找的函数
如果(cName.
HasFunction(oFunc))
        Print("The object has the function " + oFunc)
   else
         Print("The object doesn't have a function called " + oFunc)
   end
end
HasFunction(oFunc))
        打印("该对象具有函数 " + oFunc)
   否则
打印("该对象没有名为 " + oFunc + " 的函数")
   结束
结束

Much code this time but most things we know already, like Print() and ClearText() and how to get users input.
这次代码量不少，但大部分内容我们已经熟悉了，比如 Print() 和 ClearText() 以及如何获取用户输入。

Let's have a look at those parts that are new:
让我们来看看这些新增的部分：

var cName = Connect(oName)

We save a connection to an object to a variable?! Yes, kind of :). Actually this is legit and for now we don't need to know which kind of value is saved in "cName". What is important, though is to understand, that we now connected to an object (if the user typed in an existing object that is) and kind of saved this connection into a new variable.
我们将与某个对象的连接保存到变量中？！没错，差不多是这样：）。实际上这是合法的操作，目前我们无需知道"cName"中保存的是何种类型的值。但重要的是要理解，我们现在已经连接到了一个对象（前提是用户输入了已存在的对象），并将这个连接保存到了新的变量中。

And now you will (hopefully) see, why we did this:
现在你（但愿能）明白我们为何要这样做了：

if(cName.HasFunction(oFunc))

Three different things are happening here.
这里发生了三件不同的事情。

1. A function "HasFunction()" will be called at the object itself. The HasFunction() was implemented with one of the last updates to every object. It returns true if it finds the function you searched for and false if it didn't. Instead of passing the users input itself you could of course use this function in a similar way like myObject.HasFunction("SomeFunction")
1. 将在对象本身调用"HasFunction()"函数。HasFunction()是在最近一次更新中为所有对象实现的函数。如果找到您搜索的函数则返回 true，如果没找到则返回 false。除了传递用户输入本身之外，您当然也可以像 myObject.HasFunction("SomeFunction")这样使用此函数

The dot between the two expressions cName and HasFunction() actually tells the compiler, that you are using some Function of the object itself. This is important for understanding i think. You could also call some other function on it and if the object has the function implemented, it will execute.
两个表达式 cName 和 HasFunction()之间的点号实际上告诉编译器，您正在使用对象本身的某个函数。我认为这对理解很重要。您也可以调用其他函数，如果对象已实现该函数，它就会执行。

To make this more clear let me show you another example:
为了更清楚地说明，让我再举一个例子：

var computer = Connect("FinanceComputer")
var balance = computer.GetBalanceForPerson(GetNameOfCardOwner())
Print("Balance: " + balance)

This is actually what happens, if you check your balance with your credit card :)...
实际上，如果你用信用卡查询余额时就会发生这种情况 :)...顺便说一句，如果你知道正确的函数，也可以用类似的代码调整你的信用额度

So, just like in our case, a connection is established to the object called "FinanceComputer". Next the computer is addressed to call a function called "GetBalanceForPerson()". If you are able to hack your own card yet you will notice, that this function isn't available at the card itself. So it only can be called on the finance computer object. And that's what this code does.
就像我们这种情况，会建立一个连接到名为"FinanceComputer"的对象。接着计算机会被指示调用名为"GetBalanceForPerson()"的函数。如果你已经能破解自己的卡片，你会注意到这个函数在卡片本身上并不存在。所以它只能在金融计算机对象上被调用。而这正是这段代码所实现的功能。

2. Like said the HasFunction() is called on the object we adress. Also, the value/parameter for the function to search for is passed to this call --> HasFunction(oFunc) , which was the variable, that stored the users input for which function the program should look for.
2. 如前所述，HasFunction()是在我们寻址的对象上调用的。同时，要查找的函数值/参数也会传递给这个调用 --> HasFunction(oFunc)，这个变量存储了用户输入的、程序需要查找的函数名称。
3. Finally, with
3. 最后，通过

if(...)

the condition is checked, if the object, we connected to, has the function oFunc. Since HasFunction() returns true or false we can use the if/else check in this way.
该条件用于检查我们连接的对象是否具有 oFunc 函数。由于 HasFunction() 返回 true 或 false，我们可以通过这种方式使用 if/else 检查。

Last but not least we also have some slightly different way to use the Print() function:
最后同样重要的是，我们还有一种略微不同的 Print() 函数使用方式：

Print("The object has the function " + oFunc)
Print("该对象具有 " + oFunc + " 函数")

Basically it's the same Print() function as always but in this case we not just print out predefined (hardcoded) text but we also pass the variable "oFunc" to the text at the end of the line.
基本上还是那个熟悉的 Print()函数，但这次我们不仅输出预定义（硬编码）的文本，还在行末传递了变量"oFunc"。

By now, the program asks the user for an object to connect to and after this it wants him to type in the function he's looking at. For example the user wants to know if the door ("Hotel_Corridor_Door1") in his hotel room has a Say() function.
此时程序会要求用户输入要连接的对象，随后需要用户输入正在查看的函数。例如用户想知道酒店房间的门（"Hotel_Corridor_Door1"）是否具有 Say()函数。
Just to greet the player or moan about him when he passes through maybe. Or he wants to know if the fuse box in his room can Connect() and Slurp() (btw, yes it can).
可能是用来迎接玩家，或是在玩家经过时发出抱怨。又或者他想知道房间里的保险丝盒是否能执行 Connect()和 Slurp()操作（顺便说一句，确实可以）。

To end this part we want to add some code, so that the user has the ability to go back to the Main Menu again or to search for another object/function. So we add this after the whole if/else statement:
为了结束这个部分，我们需要添加一些代码，让用户能够返回主菜单或继续搜索其他对象/函数。因此我们在整个 if/else 语句后面添加如下代码：

Print("")
Print("Do you wish to search for another object?")
var chObjChoice = Input("")
if(chObjChoice == "y" || chObjChoice == "Y")
    CheckObject()
else
    MainMenu()
end
打印("")
打印("是否要搜索另一个对象？")
var chObjChoice = 输入("")
if(chObjChoice == "y" || chObjChoice == "Y")
检查对象()
否则
    主菜单()
结束

The only new thing here is the operand ||. It just adds an "Or" condition to the whole check. You also could read this like "if (chObjChoice == "y" OR "Y")
这里唯一的新东西是操作符 ||。它只是为整个检查添加了一个“或”条件。你也可以这样理解：“如果 (chObjChoice == "y" 或 "Y")”
Part IV - Passing parameters to other functions
第四部分 - 向其他函数传递参数
Let's have a look at the whole code again, that we made so far:
让我们再次看看我们目前编写的完整代码：

MainMenu()

void MainMenu()
   ClearText()
   Print("Main Menu")
   Print("=========")
   Print("")
   Print("1. Check for function")
   Print("")
   Print("Please choose an option.
MainMenu()

void MainMenu()
   ClearText()
   Print("主菜单")
   Print("=========")
打印("")
打印("1. 检查功能")
打印("")
打印("请选择一个选项。")
")
   #saves users input into mChoice
   var mChoice = Input("")
  
   if(mChoice == "1")
       Print("Excellent choice!")
      CheckObject()
   else
       Print("I'm sorry but i didn't understand " + mChoice)
       Input("Please press the 'any key'.
")
   #将用户输入保存至 mChoice 变量
   var mChoice = Input("")

   if(mChoice == "1")
打印("绝佳的选择！")
检查对象()
否则
打印("很抱歉，我没能理解 " + mChoice)
输入("请按下'任意键'。
")
       MainMenu()
   end 
end

void CheckObject()
   #clear screen again
   ClearText()
   Print("Please type in the name of the object.")
   #save users input in var oName
   var oName = Input("")
   ClearText()
   
   Print("Which function you search for?
")
       主菜单()
   结束
结束

void 检查对象()
   #再次清屏
   清除文本()
打印("请输入对象名称。")
   #将用户输入保存至变量 oName
   var oName = 输入("")
   清空文本()

打印("您要查找哪个函数？
")
   var oFunc = Input("")
    
   #connect to the object
   var cName = Connect(oName)
   
   #check if object has the function, the user searches for
   if(cName.
")
   var oFunc = 输入("")

   #连接到对象
var cName = Connect(oName)

   #检查对象是否拥有用户查找的函数
   if(cName.
HasFunction(oFunc))
        Print("The object has the function " + oFunc)
   else
         Print("The object doesn't have a function called " + oFunc)
   end
   
   #let user decide wether he wants to search for another 
   #object/function or not
   Print("")
   Print("Do you wish to search for another object?
HasFunction(oFunc))
打印("该对象具有函数 " + oFunc)
   否则
        打印("该对象没有名为 " + oFunc + " 的函数")
   结束

让用户决定是否要再次搜索
对象或函数
Print("")
Print("您希望搜索另一个对象吗？")
")
   var chObjChoice = Input("")
    
	if(chObjChoice == "y" || chObjChoice == "Y")
       CheckObject()
    else
       MainMenu()
	end
	
end
")
   var chObjChoice = Input("")

	if(chObjChoice == "y" || chObjChoice == "Y")
       CheckObject()
否则
       主菜单()
	结束

结束

Btw, if you are getting tired just hack a coffee cup :7
对了，如果你觉得累了就直接黑进咖啡杯里喝一杯吧 :7

Drink(1)
Sleepiness(-1000)

饮用(1)
困倦值(-1000)

Alright. Now that i have your attention again let's think about more options for our little program.
好了。既然重新吸引了你的注意力，现在让我们为这个小程序考虑更多选项吧。

There is something, that would be pretty useful to do but we will have to skip this until a certain bug within the game is fixed. The developer(s) , who are pretty fast replying to any issue btw, are aware of this already and hopefully will deal with it soon...
有件事本来会很有用，但我们不得不暂时搁置，直到游戏里的某个漏洞被修复。顺便说一句，开发者们回复问题的速度相当快，他们已经知晓此事，希望很快就能解决...

...until this we have to think about other things we could do.
...在此之前，我们得考虑其他能做的事情。

So, you came to Dorisburg to sell some Soda, right?! Well, actually no one wants to do this i guess ;) so wouldn't it be nice to sell some soda, without doing anything for it?
所以，你是来多丽丝堡卖汽水的对吧？！不过我觉得应该没人真想干这个 ;) 那么如果什么都不用做就能卖汽水，岂不是美滋滋？

By the time i'm writing this i noticed that this code won't work atm. There seems to be a bug with the computer that registers your sales but i hope this will be fixed by the time you read this guide.
在我写这段代码时注意到它目前无法运行。负责登记销售数据的电脑似乎存在漏洞，但希望在你读到这份指南时问题已经修复了。

EDIT: Actually it should work if you started a new game. I'm not sure why this is the case and i also checked, if i maybe had some typos. But it was the same code.
编辑：实际上如果你开新游戏的话应该是能运行的。我不确定为什么会这样，也检查过是不是有拼写错误。但代码是完全相同的。

What needs to be done? First we gonna add some new menu point:
需要做什么？首先我们要添加一个新的菜单选项：

void MainMenu()
   ClearText()
   Print("Main Menu")
   Print("=========")
   Print("")
   Print("1. Check for function")
   Print("2. Sell some Soda")
void MainMenu()
   ClearText()
打印("主菜单")
打印("=========")
打印("")
打印("1. 检查功能")
打印("2. 出售一些苏打水")

Second we need to adjust the choices:
其次我们需要调整选项：

if(mChoice == "1")
       Print("Excellent choice!")
      CheckObject()
   else if(mChoice == "2")
      SellSoda()   
   else
       Print("I'm sorry but i didn't understand " + mChoice)
       Input("Please press the 'any key'.")
       MainMenu()
   end
if(mChoice == "1")
       打印("绝佳的选择！")
检查对象()
   否则 如果(选择 == "2")
      出售苏打水()
   否则
打印("抱歉，我没理解 " + mChoice)
       输入("请按'任意键'。")
       主菜单()
   结束

So now, additionally to the condition check, if the user typed in "1" we now have an "else if" check for number "2". In that case the program calls the function SellSoda(), which (of course) we still have to write:
那么现在，除了条件检查之外，如果用户输入了"1"，我们还新增了对数字"2"的"else if"检查。在这种情况下，程序会调用 SellSoda()函数——当然，这个函数我们还需要编写：

void SellSoda()
      ClearText()
      Print("Sell a Soda(tm)")
      Print("-------------------")
      Print("")
      Print("How many Soda's you want to sell?")
      number numberOfSodas = Input("")
      string name = GetUser()   
      #todo some more code
end
void SellSoda()
      ClearText()
      Print("售出一瓶苏打水(TM)")
打印("-------------------")
打印("")
打印("您想出售多少瓶苏打水？")
数值 苏打水数量 = 输入("")
string name = GetUser()   
      #待办 更多代码
结束

First 5 lines should be understandable by now. The following two lines are somewhat different, then what we did before so far:
前 5 行现在应该可以理解了。接下来的两行与我们之前所做的有些不同：

number numberOfSodas = Input("")
string name = GetUser()

A variable "numberOfSodas" is declared and it will save the users input. But this time we don't define the type of the variable as "var" but instead as "number". Why? you may ask. Well, first things first...at this point it won't make a difference if you declare it as "var" or as "number" but in general you should think about which type of variable you really need for certain aspects of your code.
声明了一个变量"numberOfSodas"，它将保存用户的输入。但这次我们没有使用"var"来定义变量类型，而是使用了"number"。你可能会问为什么？首先需要说明的是...在目前这个阶段，无论声明为"var"还是"number"都不会有区别，但通常你应该考虑代码的特定功能真正需要什么类型的变量。

Also. As we will see soon declaring this variable as "number" has an effect straight away, compared to a declaration as "var". If the user now inputs anything else then a number the program will crash. "Bad!" you might say. And partly you'd be right :).
此外。我们很快就会发现，将这个变量声明为"number"会立即产生效果，与声明为"var"形成对比。如果用户现在输入数字以外的任何内容，程序就会崩溃。"糟糕！"你可能会说。在某种程度上，你是对的:)。

There may be some workaround(s) but for now it's just important to know that this crash will happen too (if the user types in a none-numeric value and) if you declare the variable as "var" instead of "number". It will just happen at a later point as we will see.
目前可能有些变通方案，但现阶段只需了解：如果用户输入非数值内容，且你将变量声明为"var"而非"number"，同样会发生崩溃。只不过如我们稍后将看到的，崩溃会延迟出现。

Ok enough of that...whats the second one? Right. "string name = GetUser()". This will save the name of the user in a variable, that is declared as "string". In comparison to the number issue we had before, this declaration is pretty safe for now because there's no user input at all.
好了不说这个了...第二个例子是什么？对了。"string name = GetUser()"。这会将用户名称保存在声明为"string"的变量中。相比之前遇到的数值问题，这个声明目前相当安全，因为完全不涉及用户输入。

So, how do we change the sales now? Well. If you go to the Harbour and check the computers code you should get an idea at least :). We could do something similar, that we did before like:
那么现在如何修改销售额呢？这样吧，如果你去港口查看计算机的代码，至少能获得一些思路：）。我们可以采用类似之前的做法，比如：

var sodacomputer = Connect("HarborWest_SodaStorageComputer") 
sodacomputer.RegisterSeller("Sebastian", 100)
var sodacomputer = Connect("HarborWest_SodaStorageComputer")
sodacomputer.RegisterSeller("Sebastian", 100)

But this wouldn't work. Firstly, the SodaStorageComputer itself has no function RegisterSeller(). It execudes some code, that connects to some other computer instead. Secondly.
但这行不通。首先，苏打水存储计算机本身并没有 RegisterSeller()这个功能。它执行的是连接到其他计算机的代码。其次，
Even if it would have a function like that to call, you also would have to spend the amount of money that equals the amount of sodas you selled :). But..we don't want to spend any money, right?
即使它有类似的函数可供调用，你还得支付相当于所售苏打水总价的金额：）。不过...我们可不想花钱对吧？






So let's have a look at the SodaStorageComputer again. If you register your selled Soda's there it connects 1. to the Wellspringer server to register your amount of selled Soda's and 2. it connects to the finance server to withdraw the amount of credits that equals this number.
让我们再来看看苏打存储电脑。如果你在那里登记售出的苏打，它会进行双重连接：首先连接到 Wellspringer 服务器登记售出的苏打数量，其次连接到财务服务器扣除相应数量的信用点。
Since we are actually not at this computer but somewhere else writing our own program we just need to connect to the Wellspringer server, without the need to pay for it:
由于我们实际上并不在这台电脑前，而是在其他地方编写自己的程序，我们只需要连接到 Wellspringer 服务器，无需支付费用：

void SellSoda()
      ClearText()
      Print("Sell a Soda(tm)")
      Print("-------------------")
      Print("")
      Print("How many Soda's you want to sell?
void SellSoda()
      ClearText()
打印("出售一瓶苏打水(tm)")
打印("-------------------")
打印("")
打印("您想出售多少瓶苏打水？")
")
      number numberOfSodas = Input("")
      string name = GetUser()   

       #connect to Wellspringer server
       var wellspring = Connect("Wellspringer")
       wellspring.
")
      数值 汽水数量 = 输入("")
      字符串 姓名 = 获取用户()   

      #连接到涌泉服务器
var wellspring = Connect("Wellspringer")
wellspring.
RegisterSeller(name, numberOfSodas)
       
       Print("")
       Print("The amount of " + numberOfSodas + " Soda's has been registered.")
       Input("Please press a key.
RegisterSeller(name, numberOfSodas)

Print("")
打印("已登记 " + numberOfSodas + " 瓶苏打水的数量。")
       输入("请按任意键。
")
       MainMenu()
end
")
       主菜单()
结束

Let's have a look at this:
让我们来看看这段代码：
wellspring.RegisterSeller(name, numberOfSodas)

The first part of this code should be familar by now. We connect to an object and save this into a variable. Secondly we call a function on this object called RegisterSeller(). What's new here is, that we also pass some parameters "name" and "numberOfSodas". We do need to do this because the function wants those and will not execute if they wouldn't be passed.
这段代码的第一部分现在应该很熟悉了。我们连接到一个对象并将其保存到变量中。其次，我们调用该对象上的一个名为 RegisterSeller() 的函数。这里的新内容是，我们还传递了一些参数 "name" 和 "numberOfSodas"。我们需要这样做，因为该函数需要这些参数，如果不传递它们，函数将不会执行。

The function itself could look like this:
该函数本身可能如下所示：

void RegisterSeller(string name, number amount)
     loop x from 0 to Count(arraySodaSellers)
           if(name == arraySodaSellers[x])
               arraySoldAmount[x] = arraySoldAmount + amount
           end
end
void RegisterSeller(string name, number amount)
     循环 x 从 0 到 Count(arraySodaSellers)
           if(name == arraySodaSellers[x])
arraySoldAmount[x] = arraySoldAmount + 数量
           结束
结束

Maybe by now you guess why it would be rather better to have a crash on "our" side first (number vs. var) before it hits the Wellspringer server.
或许现在你能猜到，为何在触及 Wellspringer 服务器之前，先在我们这边引发崩溃（数值与变量类型冲突）反而更为妥当。







Part V - Remote Connections / Unlocking Doors I
第五部分 - 远程连接/解锁门禁 I
Alright. Some bug was fixed recently so we can move on with our little program.
好的。最近修复了某个程序错误，我们可以继续推进这个小程序了。

Unlocking Doors via Remote Connection(s)
通过远程连接解锁门禁



In this part we want to have a look at some of the possibilities we have, when connecting to other objects. If you reat my previews parts you know, that we can just connect to some object and use his functions on his side.
在这一部分中，我们将探讨连接到其他对象时的一些可能性。如果你阅读过前面的章节就会知道，我们可以直接连接某个对象并在其端调用功能函数。
We also covered the way, how we can make sure, if an object has a certain function, that we want to use.
我们还介绍了如何确认某个对象是否具备我们想要使用的特定功能。

Especially if you want to deal with doors, this will come in handy.
尤其是在处理门类对象时，这会非常实用。

So...let's do this :) :
那么...开始吧 :)：

First we will add a new menu option to our main-menu:
首先我们将在主菜单中添加新的选项：

void MainMenu()
   ClearText()
   Print("Main Menu")
   Print("=========")
   Print("")
   Print("1. Check for function")
   Print("2. Sell some Soda")
   Print("3. Unlock a door")
void MainMenu()
   ClearText()
   Print("主菜单")
   Print("=========")
打印("")
打印("1. 检查功能")
打印("2. 出售一些苏打水")
打印("3. 解锁一扇门")

Second we need to adjust the choices again:
其次我们需要再次调整选项：

if(mChoice == "1")
       Print("Excellent choice!")
      CheckObject()
   else if(mChoice == "2")
      SellSoda()   
   else if(mChoice == "3")
      UnlockDoor()
   else
       Print("I'm sorry but i didn't understand " + mChoice)
       Input("Please press the 'any key'.")
       MainMenu()
   end
if(mChoice == "1")
       打印("绝佳选择！")
      检查对象()
else if(mChoice == "2")
      出售苏打水()   
   else if(mChoice == "3")
      解锁门()
否则
       打印("抱歉，我没理解 " + mChoice)
       输入("请按'任意键'。")
       主菜单()
结束

Of corse you also can use all the functions shown in this tutorial as standalone code. There's no need to have a menu at all for example (given, that you call it correctly and pass the right values if needed etc.) but i think for this tutorial it's a good way to do it like that.
当然，你也可以将本教程中展示的所有函数作为独立代码使用。比如完全不需要菜单（前提是你能正确调用函数并在需要时传递正确的参数等），但我认为对于本教程而言，采用这种方式是个不错的选择。

Ok, so we added a 3rth option called "Unlock door" to the menu and adjusted the if/then condition so, that it will call the function UnlockDoor(), if it's the users choice. Of course we will have to write this function still:
好的，我们已在菜单中添加了名为"解锁门"的第三个选项，并调整了 if/then 条件，这样当用户选择该选项时就会调用 UnlockDoor()函数。当然，我们还需要编写这个函数：

void UnlockDoor()
    ClearText()
    Print("Unlock a door")
    Print("===========")
    Print("")
    Print("Please type in the name of the door:")
    
    #Door name
    #it should work as string too, since it is a string but to make sure we don't
    #get any nasty bugs when connecting later on, we will use var for now
    var dName = Input("")
    var cDoor = Connect(dName)

    ClearText()
    Print("Please type in the range of numbers, that")
    Print("you want to go through:")
    Print("")
    
    #Start number
    var nMin = Input("Min: ")
    #End number
    var nMax = Input("Max: ")

    Print("Trying to unlock " + dName + " now!
void UnlockDoor()
清除文本()
打印("解锁一扇门")
打印("===========")
打印("")
Print("请输入门的名称：")

    #门名称
    #它应该也能作为字符串工作，因为它本来就是字符串，但为了确保后续连接时
    #不会出现任何讨厌的 bug，我们暂时使用 var
var dName = 输入("")
var cDoor = 连接(dName)

清空文本()
打印("请输入数字范围，该")
打印("你想输入的范围是：")
打印("")

#起始数值
var nMin = 输入("最小值：")
#结束编号
var nMax = 输入("最大值: ")

打印("正在尝试解锁 " + dName + " ！");
")
    Print("Press 'Up' or 'Down' to stop the hacking.
")
打印("按'上'或'下'键停止入侵。
")
    Sleep(2)

    BreakDoor(cDoor, nMin, nMax)
    
end
")
    等待(2)

    破门(cDoor, nMin, nMax)

结束

Quite a bit of code again but actually there's nothing, we don't know already. I will comment on some key elements either:
虽然又出现不少代码，但实际上并没有我们未知的内容。我将对一些关键元素进行说明：

var dName = Input("")
var cDoor = Connect(dName)

We catch the users input and store it in the var dName. Then we declare another var named cDoor (c for connection) and initialize it with the connection to the (game-)object, to which the doors name dName points to.
我们获取用户输入并将其存储在变量 dName 中。然后声明另一个名为 cDoor（c 代表连接）的变量，并用指向门名称 dName 所对应的（游戏）对象的连接来初始化它。

For example the user puts in "TownHall_DoorToLongsonOffice". Then the value of dName will be TownHall_DoorToLongsonOffice and the Connect() function will connect to the object, that is behind this identifier, namely the door to Longsons Office in the Town Hall.
例如用户输入"TownHall_DoorToLongsonOffice"。那么 dName 的值将是 TownHall_DoorToLongsonOffice，而 Connect() 函数将连接到该标识符背后的对象，即市政厅中通往 Longson 办公室的门。



This
这个

   BreakDoor(cDoor, nMin, nMax)

calls another function "BreakDoor()", that does the real job for us and tries to hack the door, according to the values the user typed in for the minimum and maximum numbers.
调用另一个函数"BreakDoor()"，该函数根据用户输入的最小值和最大值来执行实际工作并尝试破解门锁。

Note: Maybe it's important to clearify at this point, that we don't really "unlock" a door. We will rather try to hack it.
注意：也许在此需要澄清的是，我们并非真正"解锁"门，而是尝试对其进行破解。

We will soon see, how BreakDoor() will look like but first let's have a quick look at the parameters, which are passed to it. It's the minimum and maximum number input "nMin" and "nMax" aswell as the variable cDoor . And that's quite interesting. We actually pass the connection to an object itself (in form of a stored value within a variable) to another function.
我们很快就能看到 BreakDoor()的具体实现，但首先让我们快速了解传递给它的参数。包括最小和最大数值输入"nMin"和"nMax"，以及变量 cDoor。这一点相当有趣——我们实际上是将对象本身的连接（以变量存储值的形式）传递给另一个函数。

To be honest, i was curious too if this would work or not but it does :)
说实话，我之前也好奇这种方式是否可行，但事实证明确实有效 :)

Ok, for now we have a range of numbers from nMin to nMax, the program will try to go through later on and we have to connection to a certain door. The last part is to write the actual code, that hacks the door (or tries to):
目前我们有一个从 nMin 到 nMax 的数字范围，程序稍后会尝试遍历这些数字，我们需要连接到特定门禁。最后一步是编写实际破解门禁（或尝试破解）的代码：

void BreakDoor(var cDoor, number nMin, number nMax)
   ClearText()
    
    number counter = 0  
         
    loop x from nMin to nMax
         
         Print("Trying code: " + x)      
 
         if(cDoor.Unlock(x))
              ClearText()
              Print("Door Unlocked!")
              Print("Code: " + x)
              break
         end

      end

end
void BreakDoor(var cDoor, number nMin, number nMax)
   ClearText()

    number counter = 0

循环 x 从 nMin 到 nMax

         打印("尝试密码：" + x)      

         如果(cDoor.解锁(x))
              清空文本()
打印("门已解锁！")
打印("代码：" + x)
中断
结束

结束

结束

If you haven't work with loops yet, this is the time to do it :). But let's first take a look at how the parameters are passed to the function:
如果你还没接触过循环语句，现在正是时候 :)。不过让我们先来看看参数是如何传递给函数的：

void BreakDoor(var cDoor, number nMin, number nMax)

First of all, you can name the paramaters, that the function needs as input in another way. Meaning it must not be the same, as the once you declared somewhere else in your program and then pass to it.
首先，你可以用不同的方式命名函数所需的输入参数。这意味着它们不必与你在程序其他地方声明并传递给函数的参数同名。
Actually it's rather not recommended to name them in exact the same way but for now i think it'll make things easier to understand.
实际上，并不建议使用完全相同的名称来命名它们，但目前我认为这样会让理解变得更容易。
But to be more specific on that you could name them also like:
但更具体地说，你也可以这样命名它们：

void BreakDoor(var someDoor, number minimum, number maximum)

I won't go any deeper into this but for now let's just note, that the variables (the parameters the function needs as input) you declare here are actually new ones.
我不会对此深入探讨，但我们现在只需注意：此处声明的变量（函数所需的输入参数）实际上是全新的变量。
If call this function from somewhere else within your program code, the usage BreakDoor(cDoor, nMin, nMax) will actually not really pass the variables cDoor, nMin and nMax but instead just their (stored) values.
如果在程序代码的其他地方调用此函数，使用 BreakDoor(cDoor, nMin, nMax) 实际上并不会传递变量 cDoor、nMin 和 nMax，而只是传递它们存储的值。

What's also interesting is, that we previously declared nMin and nMax as "var" in our function UnlockDoor(). But the function BreakDoor() itself wants "number" instead. Still this works because (i'm guessing here) the type of value "var" will implicitly be converted to the type "number" in this case.
同样有趣的是，我们之前在函数 UnlockDoor() 中将 nMin 和 nMax 声明为 "var" 类型。但 BreakDoor() 函数本身需要的是 "number" 类型。这仍然可行，因为（我猜测）在这种情况下，"var" 类型的值会被隐式转换为 "number" 类型。

Let's move on to the loop:
现在让我们继续讨论循环：

loop x from nMin to nMax
   #do something
end

循环 x 从 nMin 到 nMax
   #执行某些操作
结束

Maybe you already saw this ingame or reat the disc, referring to loops. And, well, in principle it's really no big deal but you should be careful though because with loops you can pretty much ♥♥♥♥ up the program at run-time if you create an endless loop for example.
或许你已在游戏中见过或读过相关说明，指的就是循环结构。实际上这本身并不复杂，但需要格外注意——比如若创建了无限循环，很可能在程序运行时彻底搞砸整个系统。

What it does is simply looping a segment of code "nMax" times, starting at "nMin".
其作用仅仅是循环执行一段代码 "nMax" 次，从 "nMin" 开始。

The "segment of code" in our case is:
我们这里的"代码段"是：

 Print("Trying code: " + x)      
 
         if(cDoor.Unlock(x))
              ClearText()
              Print("Door Unlocked!")
              Print("Code: " + x)
              break
         end

 Print("尝试代码：" + x)      

         if(cDoor.Unlock(x))
清空文本()
打印("门已解锁！")
打印("代码：" + x)
跳出
结束

It will call the function Unlock() at the door, we connected to using the actual value "x" as code-number, while "x" itself will be raised by one each time until the number "nMax" is reached.
它将调用门上的 Unlock()函数，我们使用实际数值"x"作为密码编号进行连接，同时"x"本身会每次递增 1，直到达到"nMax"这个数值。

In other words we just try any number from nMin to nMax to open the door.
换句话说，我们只是尝试从 nMin 到 nMax 之间的每个数字来打开这扇门。

Since Unlock() returns true or false we can use it directly within an if condition check:
由于 Unlock()返回 true 或 false，我们可以直接在 if 条件检查中使用它：

  if(cDoor.Unlock(x))

如果(cDoor.Unlock(x))

and if we are lucky and unlocked the door we stop the loop with:
如果我们运气好成功解锁了门，就通过以下方式终止循环：

break
Part V - Remote Connections / Unlocking Doors II
第五部分 - 远程连接 / 解锁门篇 II
That's pretty much what you will need to brute force a door to unlock it. Let's quick get back at some point in our UnlockDoor() function we call, when the user chooses this from the main menu:
以上就是暴力破解门锁所需的全部操作。让我们快速回到调用 UnlockDoor()函数的某个节点，当用户从主菜单选择此选项时：

Print("Press 'Up' or 'Down' to stop the hacking.")
Sleep(2)

打印("按'上'或'下'键停止破解。")
休眠(2)

You may have wondered what this is for :). Well, i'll get back to this in a second but let's first complete the UnlockDoor() function so that the user is able to hack another door or go back to the main menu after this:
你可能好奇这段代码的用途：）。稍后我会详细解释，但让我们先完成 UnlockDoor()函数，这样用户就能在操作后继续破解其他门锁或返回主菜单：

After
之后

BreakDoor(cDoor, nMin, nMax)

we put this code
我们放入这段代码

Print("Search for another door (y/n)?")
var uChoice = Input("")
if(uChoice == "y")
  UnlockDoor()
else
  MainMenu()
end

Print("搜索另一扇门（是/否）？")
var uChoice = Input("")
if(uChoice == "y")
  UnlockDoor()
else
主菜单()
结束

So, if the user hacked a door, meaning if "BreakDoor()" was executed, the program will jump back to UnlockDoor() and executes the code above.
因此，如果用户破解了门锁，即执行了"BreakDoor()"函数，程序将跳转回 UnlockDoor()并执行上述代码。

Hacking a door can be pretty boring because most doors in Else.Heartbreak() have a door code of at least 5 digits and it can take a while, till you looped to the right position.
破解门锁可能相当枯燥，因为《Else.Heartbreak()》中大多数门的密码至少有 5 位数，可能需要循环尝试很久才能找到正确位置。

The user should at least be able to abort the hacking process by pressing a certain key.
用户应至少能够通过按下特定按键来中止入侵进程。

So let's expand our BreakDoor() function a bit for this purpose:
为此，让我们扩展一下 BreakDoor() 函数：

void BreakDoor(var cDoor, number nMin, number nMax)
   ClearText()
   number counter = 0  
   loop x from nMin to nMax
          
        if(IsKeyPressed("down") || IsKeyPressed("up"))
void BreakDoor(var cDoor, number nMin, number nMax)
   ClearText()
计数器数值 = 0
循环变量 x 从 nMin 到 nMax

        如果（按下按键"下" || 按下按键"上"）
            break
            跳出循环
        end

        Print("Trying code: " + x)      
        if(cDoor.Unlock(x))
              ClearText()
              Print("Door Unlocked!")
              Print("Code: " + x)
              break
         end
      end
end
结束

        打印("尝试代码：" + x)      
        如果(cDoor.解锁(x))
              清除文本()
打印("门已解锁！")
打印("代码：" + x)
中断
结束
结束
结束

With IsKeyPressed() we will watch the users input while the program runs the loop and if he presses the down or up arrow key the loop will stop (break).
通过 IsKeyPressed() 函数，我们将在程序运行循环时监测用户输入，若用户按下向下或向上方向键，循环将停止（中断）。

As last part of this section i want to show a slightly different approach to hack a door.
作为本节的最后部分，我想展示一种略微不同的破解门禁的方法。
Instead of just running from let's say 0 to 99999 it would be cooler to have the numbers jump from the smallest to the highest number, then to the smallest number +1 again, followed by the highest number -1 and so on. This way you might get quicker results at some doors.
与其只是从 0 到 99999 这样顺序运行，不如让数字从最小值跳到最大值，再跳回最小值+1，接着是最大值-1，依此类推。这样或许能在某些门禁处更快获得结果。

add
添加

if(Mod(x,2)!=0)
   x = nMax - counter
   counter = counter +1
else 
   x = counter
end
如果(取模(x,2)!=0)
   x = 最大值 - 计数器
计数器 = 计数器 + 1
否则
   x = 计数器
结束

before the line:
在代码行之前：
Print("Trying code: " + x)
Print("正在尝试代码：" + x)

We declared the counter outside of the loop and initialized it with the value 0. Now
我们在循环外部声明了计数器并将其初始化为 0。现在

Mod(x,2)

does nothing else then to divide the actual value of x by 2 and return the rest of this division. If the rest is "0" then it's an equal number, if not it's an unequal number.
仅用于将 x 的实际值除以 2 并返回该除法的余数。若余数为"0"则为偶数，否则为奇数。
Wether x is equal or unequal it will be assigned by the value of the highest number (nMax) - the actual counter value or to the value of the counter itself.
无论 x 是偶数还是奇数，都将被赋值为最大数值（nMax）减去当前计数器的值，或是直接赋值为计数器自身的值。
If the inital values for nMin and nMax would be 0 and 99999, this way we would jump from 0 to 99999, then back to 1, then to 99998, back to 2 and so on.
如果 nMin 和 nMax 的初始值分别为 0 和 99999，通过这种方式我们将从 0 跳至 99999，然后回到 1，再到 99998，接着回到 2，依此类推。



5 条留言

Arucard 2017 年 6 月 8 日 上午 8:33 
Hello from the future! I still don't own this on Steam, so can't rate it up, but have favorited'd and want you to know how amazing this guide is. Using this has helped me understand a lot and prompted me to start earnestly learning programming.
来自未来的问候！我仍未在 Steam 上拥有这款游戏，因此无法评分，但已收藏此指南并想告知您这份指南有多么出色。使用这份指南让我理解了许多知识，并促使我开始认真钻研编程。
Great game, great guide, great googly moogly I am in your debt, sir.
游戏很棒，指南很棒，真是天赐良物啊老兄，我欠你个人情。

Balthasar  [作者] 2016 年 2 月 26 日 上午 5:01 
I think you don't "need" to do anything like this. It's more like a sandbox outlined with some basic story. But the main goal here (at least as far as I know) is to get ppl to learn how to code and stuff.
我认为你并不"必须"做这类事情。这更像是一个用基础故事勾勒出的沙盒世界。但主要目标（至少据我所知）是让人们学习编程之类的技能。
Tutorials like this just aim to give you some direction what is possible and where to start if you don't know what you want to do next.
这类教程旨在为你指明方向，当你不知道下一步该做什么时，告诉你有哪些可能性以及该从何入手。

Kakkamakkara 2016 年 2 月 26 日 上午 3:04 
So does one need to actually do ♥♥♥♥ like this in the game? I'm out.
所以真的需要在游戏里做这种破事吗？我溜了。

Balthasar  [作者] 2016 年 1 月 30 日 上午 7:52 
Thx :). Nice game, man!
谢啦：）。哥们，这游戏真不错！

Erik 2016 年 1 月 23 日 上午 5:47 
Wow, this is super impressive!
哇，这简直太厉害了！
Valve 徽标
© Valve Corporation。保留所有权利。所有商标均为其在美国及其它国家/地区的各自持有者所有。 本网站上部分地理空间数据由 geonames.org 提供。
隐私政策   |  法律信息   |  无障碍  |  Steam 订户协议  |  Cookie
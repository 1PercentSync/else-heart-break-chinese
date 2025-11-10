SPRAK Manual
由 Cooper 制作
A copy of the in-game manual for ease of reference.
*BONUS* Includes a list of in-game sound effects
 6




 
 
奖励
收藏
分享
创建者

Cooper
离线
类型: 基本游戏玩法, 攻略
Languages: 英语
发表于
更新日期
2015 年 11 月 3 日 上午 4:54
2015 年 11 月 17 日 上午 5:11
4,453	不重复访客数
193	当前收藏人数
指南索引
总览
Contents 
Chapter 1 --- Introduction to problem solving 
Chapter 2 --- Variables 
Chapter 3 --- Math 
Chapter 4 --- Functions 
Chapter 5 --- If statements 
Chapter 6 --- Arrays 
Chapter 7 --- Loops 
Chapter 8 --- Manipulating text 
Chapter 9 --- Objects and methods 
*BONUS* List of in-game sound effects 

留言
Contents
##########################################
# SPRAK PROGRAMMING LANGUAGE MANUAL #
##########################################

Programming, what a wonderful thing!
Ah... the joy of controlling everything and
anything through the magical spells of code alone.

This manual will teach you all you need to
know to become a sorcerer of bits, a
a master of logic and the ruler of both
data and functions.

There are 10 chapters and we recommend
you read them all in order, since
the later chapters build upon previous material.
Here's an overview of what we will go through:

------------------------
1. Introduction to problem solving
2. Variables
3. Math
4. Functions
5. If-statements
6. Arrays
7. Loops
8. Handling text
9. Objects and methods
------------------------

The language you will learn is called SPRAK.
We hope that you will find it fun and
enlightening to use!
Chapter 1 --- Introduction to problem solving
=============================================

How do you solve a problem? This is a tricky question
for sure. After all, every problem is unique and
needs its own unique solution. Or does it?

A lot of similar, related problems in programming
come up again and again. An experienced programmer
might not have to think consciously at all about
how to solve such situations. The right answer just
appears in her or his head and can be typed into
the computer without much thought at all.

But how does one learn how to perform such a feat?
"Read code!" is an oft-repeated mantra for
the young and inexperienced programmer. This tip
is very good, naturally, but can be hard to follow
in practice. Many real-world solutions are both messy
and poorly structured, with many strange details that
make them both hard to read and reason about.

Without guidance a page of code might be almost
impossible to understand for anyone except its author
(and sometimes even her or him). Still, it IS very much
worthwhile trying to understand other people's code and
pick up little things here and there, trying to find
patterns and exploring another person's solutions.

A very good trick indeed can be to rewrite someone
else's code with your own style and improvements.
This helps tremendously with understanding the flow
and connections in the written code.

But back to problem solving! The main method of
solving a problem, in programming and otherwise,
is to DECONSCTRUCT the problem and solve each
component individually. This simple method has proven
to be incredibly effective and lies at the heart
of both functional composition (see Chapter 5) and
object orientation (see Chapter 9).

By breaking things down into smaller and smaller
pieces, something big and tangled can become quite
small and manageable. How to do this well is something
we will touch upon again and again throughout this
text. By analyzing the problem at hand and then trying
to take it apart and putting it together with the
different constructs you will learn in the following
chapters, hopefully you too can become a master of
problem solving!

Finally, if nothing works and a solution completely
eludes you - take a nap, walk away from your machine,
go for a walk or grab a sandwich. Nothing helps
problem solving as much as sleep and some fresh air.
Chapter 2 --- Variables
=======================

Variables are used for naming things. This might
seem like a trivial task but do not be deceived,
the ability to name things is one
of the most important things of all in programming.
The reason for this is that it allows us to
abstract and handle the components of our solution
in a symbolic way.

For example: instead of referring to your name as
a string of characters like "Lisa" we can create
a variable containing this value. The variable
might be called something like 'name' and could
even refer to different names at different points
in time, depending on the needs of the program.

Here are some examples of how to create variables:

number x = 10
string myName = "Simon"
bool itIsChristmas = false

Notice the words to the left, like 'number', 'string'
and 'bool'. Those are TYPES and they describe what
kind of data we want to put inside the variable.
After that comes the variable name. This should be
something descriptive that explains what kind of thing
we have put inside the variable.

Next up is the equal sign, '='
This means that we set the value of the variable to
whatever is on the right side.

Variables can be re-set to other values by using the
equal sign, like this:

x = 11
myName = "Simone"
itIsChristmas = true

This requires the variables to have already been declared
earlier in the program using the syntax above with
a type and a name (like 'number x = 10' for example).

If a variable contains a number type it can be changed
in some additional ways just for convenience, saving
a few key strokes:

x++ # Increase by one
x += 10 # Increase by 10
x-- # Decrease by one
x -= 20 # Decrease by 20
Chapter 3 --- Math
==================

Don't let this chapter scare you! You don't
need to be a math whiz to do programming.
Curiosity and logical thinking will take you
much further, trust me.

Still, it is often very useful to perform
different kinds of math operations in your
code.

The basic ones are:

+ # plus
- # minus
* # multiplication
/ # division

These operations can be used in the
following manner:

number x = 10 + 20
number y = x * 2
number z = y - 1

Now x will have the value 30, y will be 60
and z will be 59. Make sure you understand
why this is and how each variable depends
on the one defined before it.

You can also construct more complex, compound
math statements like the following one:

x = 10 / y * z + 1

The operators follow regular operator precedence,
which means that the * and / will be calculated
before + and - .

To change this, parenthesis can be used:

x = 10 / y * (z + 1)

Different computer systems might have other math
operations available as FUNCTIONS (see Chapter 5).

There are some more math operators that are quite
handy, like the following ones:

< # less than
<= # less than or equal
> # greater than
>= # greater than or equal
== # equal
!= # not equal

They are used similarly to +, -, * and / but
return true or false as their result:

30 < 20 # false
5 <= 5 # true
100 != 0 # true
Chapter 4 --- Functions
=======================

Functions are the main tool for abstraction
and simplification in programming. Basically
it gives your code super powers by making
something a self contained unit and parameterizing
it. This all sounds very abstract and perhaps
not very useful in itself, so let's check out
some examples.

First of all, here's how you CALL a function:

Print("Hello!")

This will make the computer print the text "Hello!"
(without the quotes) to the screen - if it is a
computer with a monitor, that is!

In that case the name of the function was Print
and the parameter was "Hello!". By using different
parameters we can make the function behave
differently each time we call it.

This function might seem a bit strange, at least
if you're used to functions in math. The weird thing
is that it doesn't return anything! Functions
in SPRAK don't need to do that, so that's totally fine.

We often want to use functions that return
values though, so here's how:

number x = Max(13, 10)

This will assign the return value from the function Max
to the variable 'x'. Max takes two parameters and returns
whichever one is the largest. In this case it is 13 so
'x' will get that value.

We could also use variables or other functions as
parameters to the Max function, like this:

number x = Max(Random(), 0.5)

The Random function doesn't take any parameters and
returns a random value between 0.0 and 1.0. This random
value is compared to 0.5 and the biggest one is selected
and assigned to 'x'.

Writing your own functions is pretty simple and
looks a bit like defining variables. You could even
say that a function is a variable containing a piece
of code, so it makes sense to think about it that way.

Here's my attempt at writing my own function for
calculating the area of a circle:

number AreaOfCircle(number radius)
return radius * radius * 3.1415
end

There are many different parts to this code so let's
pull it all apart. The first line explains what kind
of value the function will return (a number), the name of
the function itself ('AreaOfCircle') and finally what
parameters it accepts (a single number called 'radius').

The names used for the parameter is only known inside
the function - someone using the AreaOfCircle function does
not need to know anything about it.

Inside the function, between the first line and the last
one containing the final 'end', we have the code that
will run when this function is called. In this particular
case we multiply 'radius' with itself and with Pi,
then return that particular result to the caller.

To use this function we could do something like this:

number a = AreaOfCircle(3.0)

The variable 'a' would now contain the value
of 3.0 * 3.0 * 3.1415, which is something like 28.27

If you want to see if an object has a certain function
you can use the function HasFunction(), like this:

if HasFunction("Print")
Print("Hello")
end
Chapter 5 --- If statements
===========================

Time for some fun! The if statement is
a central part of what makes computers
able to "think". It selects different paths
through the code, depending on if
something is true or not. Kind of like
a railroad switch with a little guy
checking which way the current train should go.

For example, let's say that we have a variable
named 'SECRET', containing either the value
'true' or 'false', we don't know which.

How can we find out?!
By using an if-statement of course:

if SECRET
Print("true!")
end

There are several interesting things to note
here. First of all the use of the two words
'if' and 'end'. These two go together like
a pair. The 'end' word shows where the block
of code ends. Everything in between will happen
if the variable called 'SECRET' contains the
value 'true'. In this case it will print "true!".
On the other hand, if the variable contains
'false' nothing will happen at all.

What if we want to print something in that
case too? Here's one way to do it:

if SECRET
Print("true!")
else
Print("false!")
end

You could do several things in the block
between 'if' and 'end'. Don't be confused by
the text being moved a bit to the right, that's
just for readability.

if SECRET
Print("The")
Print("SECRET")
Print("is")
Print("true!")
end

It is possible to have even more cases
by using the following notation:

if a
# do something
else if b
# do something else
else if c
# do this if c is true (but a and b are false)
else
# do this if a, b and c are all false
end


Finally, several variables or expressions
containing logical values (true / false)
can be checked together using the word 'and'
and 'or':

if a and (b or c)
# this will happen if a and b
# OR a and c are true
end

Don't worry if this seems confusing, just
come back later and re-read any section
that didn't make sense the first time.
Chapter 6 --- Arrays
====================

Arrays are a way of combining several values into
one structure that can be manipulated and passed
around as a single unit. It's very handy!

Here's one way to create an array:

array numbers = [10, 5, 7, 123]

The '[' and ']' surround the numbers or whatever
you want to put into the array, kind of like the
sides of a box or something...

This array (called 'numbers') now contains the
four numbers 10, 5, 7 and 123. We can get them out
of the variable by using the indexing operator,
which looks like this:

Print(numbers[0])
Print(numbers[1])
Print(numbers[2])
Print(numbers[3])

This will print each one of the "slots" in the array,
starting with the '10' in the first slot and ending with
'123' in the last one. Note that the indexing of the
array starts at 0 and ends at 3. This is a weird standard
that most programming languages follow and you just
need to accept that it feels a bit strange.

The indexing operator can also be used to assign new values
to somewhere in the array:

numbers[1] = 666

Now the 'numbers' variable will contain [10, 666, 7, 123]
Be careful with this so you don't create bugs! Manipulating
arrays in this way can be tricky.

There is a function called 'Count' that
counts how many things there are in an array.
Just pass the array to 'Count' like this:

Count(numbers) # the function returns 4 in this case

There are more tricks and functions that use
arrays but this is a good start.
Chapter 7 --- Loops
===================

Loops are used to make a block of code
run over and over again, like this:

loop
Print("Hello!")
end

This will keep on printing "Hello!" forever
and ever, or at least until someone turns off
the computer, or something like that...

A loop can also be interrupted from inside
using the 'break' keyword:

loop
if IsDone()
break
end
end
Print("Done!")

This code will keep looping until the function
'IsDone' returns true, upon which execution will
continue below the loop and the program
will print "Done!".

A loop can also loop "through" an array. This is
very simple, just add the array after the 'loop' keyword:

loop [1, 2, 3, 4, 5]
# do something five times
end

Usually you want to access the elements in the array.
Then you have to give it a name:

array a = [1, 2, 3, 4, 5]

loop x in a
Print(x)
end

This will print each thing that is stored in the array!
If you want to loop through a range of numbers, say 0 up
to 100 you can do that with the 'from' ... 'to' keywords:

loop x from 0 to 100
Print(x)
end

This is much more efficient than creating an array with
all those numbers in it.
Chapter 8 --- Manipulating text
===============================

SPRAK's tools for manipulating text
are few and kind of simplistic. It's still
important to know them though. Without
good textual output and input a lot of
programs would suffer from very poor usability.

The most important operation is representing
different kinds of data as text.
This is usually automatically done whenever
a function needs text. For example
the print function:

Print(10)

This will output the text "10", just as expected.
Somewhat more interesting is the '+' operator
which can be used to put together longer
strings:

Print("X has the value " + X)

This will automatically convert X to a
string and print something like
"X has the value 42", if X has the value 42.

You can also print arrays but if they are
long they might get cut off half-way through
to fit on the screen and not create
enormous strings.
Chapter 9 --- Objects and methods
=================================

This is the final and perhaps most exciting
chapter of this whole manual. We have now
reached the most powerful construct of
Sprak, the dot operator.

The dot operator is used to call functions
in OTHER objects. These functions are usually
called methods to differentiate them from
normal functions.

Since such a method can do things to
the object it is attached to, and even call
other methods in other objects, the power
that comes with the dot operator is immense.

Please wield it carefully and try not to make a mess.

Here's how you use it. First connect to
another object:

var o = Connect("HomeComputer")

Then use the connection
(stored in the variable 'o' in this case)
together with the dot operator to call a method:

o.Print("Hello at home!")

This will make the computer that you connected
to print the message "Hello at home!" on its
monitor.

This requires access to the internet, naturally.
*BONUS* List of in-game sound effects
Call with:
PlaySound()
e.g
PlaySound("Powerup 3")

"Blip 1"
"Blip 2"
"Blip 3"

"Coin 1"
"Coin 2"
"Coin 3"
"Coin 4"

"Error"
"Warning"

"Explosion 1"
"Explosion 2"

"Hit 1"
"Hit 2"
"Hit 3"

"Jump 1"
"Jump 2"
"Jump 3"

"Laser 1"
"Laser 2"

"Win"
"Lose"

"Powerup 1"
"Powerup 2"
"Powerup 3"
"Powerup 4"
"Powerup 5"

"Shoot 1"
"Shoot 2"
"Shoot 3"

"Arcade music"

"Sine"
"Square"
9 条留言

rozsa 9 月 24 日 上午 8:50 
@herr horny parentheses have different meaning in different context -- in the games title, () after "Break" means the function "Break" is called

Herr Saney 2024 年 11 月 18 日 上午 10:31 
Can somebody explain me in a way of the biggest coding noob you can imagine what () means?

changoide 2023 年 9 月 30 日 上午 10:07 
ditto, the intro to problem solving is absolutely worth reading

۩۞۩Jpyellow۩۞۩ 2022 年 3 月 11 日 上午 10:34 
Thank you for this

Wunderforce 2017 年 3 月 15 日 上午 10:42 
As a programmer, I can say that the information on problem solving is totally wise words to live by.

Overdrive 2016 年 1 月 6 日 下午 8:37 
Not bad not bad.
~~(8:>

Geeky Meerkat 2015 年 12 月 29 日 下午 1:30 
I'm not sure if it's documented any where, but if you have an array you can do things like this:
[code]
loop ArrayName
Print(@)
end
[/code]

And that will go through each of the elements of the array, and @ will be the contents of that specific array item.

RavenWorks 2015 年 11 月 30 日 上午 5:07 
Some more sound effects not listed here:

"Modem 1"
"Modem 2"
"Modem 3"

"Startup"

"Complete 1"
"Complete 2"

"Fail 1"
"Fail 2"

"Bird"

"Blackbird 1"
"Blackbird 2"
"Blackbird 3"
"Blackbird 4"
"Blackbird 5"

"Electricity 1"
"Electricity 2"

"Guardian"

"ComputerSound 1"
"ComputerSound 2"
"ComputerSound 3"
"ComputerSound 4"

"Thunder 1"
"Thunder 2"

"Atmosphere 1"
"Atmosphere 2"

HariSeldon 2015 年 11 月 6 日 上午 2:22 
Thank you so much for posting this.
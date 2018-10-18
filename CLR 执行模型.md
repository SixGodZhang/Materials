# CLR 执行模型

## 目录
- [将代码编译成托管代码](#将代码编译成托管代码)
- [将托管代码合并成程序集](#将托管代码合并成程序集)
- [加载公共语言运行时](#加载公共语言运行时)
- [执行程序集的代码](#执行程序集的代码)
- [本机代码生成器:NGen.exe](#本机代码生成器:NGen.exe)
- [一些专业名词解释](#一些专业名词解释)

## 将代码编译成托管代码
---
### 为何要将代码编译成托管代码?
因为 托管代码可以由CLR(目前作为.Net Framework的一部分)调用去执行相关逻辑.
CLR 核心功能: 
 - 内存管理
 - 程序集加载
 - 安全性
 - 异常处理
 - 线程同步

只要是面向CLR的语言，都可以由CLR来管理这些代码的执行,这是支持跨平台的主要原因.

编译器可以理解为语法检查器和代码分析器,比如说Visual Studio. 任何支持CLR语言的源代码在经过编译器之后都会生成托管模块.

![支持CLR的语言编译成托管代码](https://github.com/SixGodZhang/Materials/blob/master/Images/sourecode2managecode.png)

托管模块是标准的Windows的可移植执行体(PE32(+))文件.

![托管模块的组成部分](https://github.com/SixGodZhang/Materials/blob/master/Images/partsofmanagemodule.png)

## 将托管代码合并成程序集
---
程序集是一个抽象的概念,是一个或多个模块/资源文件的逻辑性分组.其次,是重用、安全性以及版本控制的最小逻辑单元.通过AL.exe可以将一组文件合并到程序集.
![将托管模块合并成程序集](https://github.com/SixGodZhang/Materials/blob/master/Images/manageModulemergeassembly.png)

## 加载公共语言运行时
---
.Net Framework 本地安装目录:
- C:\Windows\Microsoft.NET\Framework
- C:\Windows\Microsoft.NET\Framework64

Microsoft SDKs提供了clrver.exe可以查看CLR的版本号,-all 参数可以查看正在运行的程序使用的clr版本号
![clrver](https://github.com/SixGodZhang/Materials/blob/master/Images/clrver.png)
![clrver](https://github.com/SixGodZhang/Materials/blob/master/Images/clrver-all.png)


## 执行程序集的代码
---
![程序集的执行过程](https://github.com/SixGodZhang/Materials/blob/master/Images/ExcuteProcess.png)

托管程序集:
- PE32(+)头
- CLR头
- 元数据
- IL

工具:
- ILAsm.exe：IL汇编器
- ILDasm.exe: IL反汇编器
- PEVerify.exe: 验证程序集，并报告其中含有不安全代码的方法

注意:高级语言一般只公开了CLR的部分功能,IL可以操作CLR的全部功能.

在运行时，会将IL代码转换成本机CPU指令.这部分功能事由CLR中的JIT(just in time) 来完成的.
![第一次调用方法](https://github.com/SixGodZhang/Materials/blob/master/Images/firstcallmethod.png)

描述(以调用Main方法举例):在调用Main方法之前,会检查Main方法中引用的所有类型.这导致CLR分配一个内部数据结构来管理对引用类型的访问.
如上,在Main中调用Console类型,CLR就会分配一个数据结构来管理对Console的访问,Console类型中的每一个方法都有对应的记录项,每个记录项中都包含了
一个对应的地址,根据该地址，可以找到方法的实现.对这个结构初始化的时候,CLR将每个记录项中对应的地址都指向一个未编档的函数。该函数被称为,JITCompiler,
JITCompiler负责将IL代码编译成本机CPU指令.JITCompiler会在定义的程序集中根据元数据查找对应的IL代码,接着JIT会验证IL代码，并将IL代码编译为本机CPU指令.
本机CPU指令会被保存在动态分配的内存中,然后JITCompiler会将Console中对应的记录项指向在动态内存中的本机CPU指令.第二次调用可以直接取记录项对应的本机CPU指令,
而不会再调用JITCompiler.

注意:CLR中的JIT还会对本机代码进行优化.
两个C#编译器开关会影响代码优化:
- /optimize
- /debug

![优化后的IL代码](https://github.com/SixGodZhang/Materials/blob/master/Images/optimize.png)

![未优化的IL代码](https://github.com/SixGodZhang/Materials/blob/master/Images/no-optimize.png)

optimize 控制是否优化 IL代码,debug 控制是否优化 本机CPU指令.

/optimize-(默认):此命令不会优化 IL代码.在未优化的IL代码中,包含很多NOP(no-operation空指令),还包含许多跳转到下一行代码的分支指令.Visual Studio利用这些指令在
调试期间提供"编辑并继续的功能".

/debug(+/full/pdbonly) :编译器会生成PDB文件,供调试器使用


## 本机代码生成器:NGen.exe
---
将IL代码生成本机代码

## 一些专业名词解释
---
CLR: Common Language Runtime, 公共语言运行时

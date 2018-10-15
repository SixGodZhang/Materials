# CLR 执行模型

## 目录
- [将代码编译成托管代码](#将代码编译成托管代码)
- [将托管代码合并成程序集](#将托管代码合并成程序集)
- [加载公共语言运行时](#加载公共语言运行时)
- [执行程序集的代码](#执行程序集的代码)
- [本机代码生成器:NGen.exe](#本机代码生成器:NGen.exe)
- [FrameWork类库](#FrameWork类库)
- [通用类型系统](#通用类型系统)
- [公共语言规范](#公共语言规范)
- [与非托管代码的互操作性](#与非托管代码的互操作性)
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

两种工具:
- ILAsm.exe：IL汇编器
- ILDasm.exe: IL反汇编器

注意:高级语言一般只公开了CLR的部分功能,IL可以操作CLR的全部功能.

![第一次调用方法](https://github.com/SixGodZhang/Materials/blob/master/Images/ExcuteProcess.png)

## 本机代码生成器:NGen.exe
---

## FrameWork类库
---

## 通用类型系统
---

## 公共语言规范
---

## 与非托管代码的互操作性
---

## 一些专业名词解释
---
CLR: Common Language Runtime, 公共语言运行时

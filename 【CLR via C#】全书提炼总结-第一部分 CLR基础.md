# 【CLR via C#】全书提炼总结-第一部分 CLR基础

## 概要：
- [CLR的1执行模型](#CLR的执行模型)
- [生成、打包、部署和管理应用程序及类型](#生成、打包、部署和管理应用程序及类型)
- [共享程序集，和强命名程序集](#共享程序集，和强命名程序集)

## CLR的执行模型

__IL代码模型__:
![assemblyPart](https://github.com/SixGodZhang/Materials/blob/master/Images/assemblyPart.png)
![codepart](https://github.com/SixGodZhang/Materials/blob/master/Images/codepart.png)

__基于CLR应用的加载过程__:  
1. 通过点击exe或则其它方式触发Window开始为应用创建进程(Windows会根据EXE的文件头的信息，来判断是创建32位or64位进程)  
![targetPlatform](https://github.com/SixGodZhang/Materials/blob/master/Images/targetPlatform.png)

2. 之后再进程的地址空间中加载MSCorEE.dll,这个MSCoreEE.dll有两个版本,32位or64位(这个文件被称之为'垫片'),32位的位于C:Windows\SystemWoW64,64位的位于C:Windows\System32  
3. 然后,MSCorEE.dll会检测可执行文件使用的CLR的版本信息,根据其版本，加载相应的CLR到进程中。(注意:不同版本的CLR的文件名称是不一样的)  
- 版本1.0 C:\Windows\Microsoft.NET\Framework\v1.0.3705
- 版本1.1 C:\Windows\Microsoft.NET\Framework\v1.1.4322
- 版本2.0 C:\Windows\Microsoft.NET\Framework\v2.0.50727
- 版本4 C:\Windows\Microsoft.NET\Framework\v4.0.30319

![netframeworkversion](https://github.com/SixGodZhang/Materials/blob/master/Images/netframeworkversion.png)  

注意:其中.NET Framework3.0和3.5 是和 CLR2.0一起发布的,CLR DLL加载是从v2.0.50727中进行加载的  

4. 最后，CLR会加载EXE的程序集到进程中，并找到程序的入口点Main,进而执行相关的代码  

__基于CLR的代码执行过程__:  
1. 在调用 某个方法时,CLR首相会检测出该方法引用的所有类型,并为对应的类型分配一个数据结构来管理对类型的引用  
2. 在该数据结构内部，所有成员都包含一个记录项。如果该记录项是方法，则其包含一个地址，该地址指向该方法的实现。在方法栈的顶端，也就是在方法的入口处，该地址会指向一个未编档函数，该函数被称为JITCompiler  
3. 如果记录项中的方法是第一次被执行，则就会调用未编档函数JITCompiler,在JITCompiler中,首先会在元数据表中查找到该方法的记录，根据此记录又可以找到相应的IL代码,然后CLR会分配一块内存空间,将IL编译成本机代码之后存放在该内存中，并且上述提到的记录项中的地址由指向JITCompiler改为指向该内存地址  
4. 如果记录项中的方法是第n(n>1)次调用,则会直接通过记录项中的地址找到本机代码，不会再经过JITCompiler  

![codeProcess](https://github.com/SixGodZhang/Materials/blob/master/Images/codeProcess.png)  


## 生成、打包、部署和管理应用程序及类型

## 共享程序集，和强命名程序集

# 【CLR via C#】全书提炼总结-第一部分 CLR基础

## 概要：
- [CLR的1执行模型](#CLR的执行模型)
- [元数据概述](#元数据概述)
- [弱命名程序集和强命名程序集](#弱命名程序集和强命名程序集)
- [全局程序集缓存GAC](#全局程序集缓存GAC)
- [CLR定位程序集](#CLR定位程序集)

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

## 元数据概述
托管PE文件由四部分组成:
PE32(+)头、CLR头、元数据、IL
- PE32(+)头，是Windows要求的标准信息
- CLR头：是需要CLR支持的特有的模块，包含CLR的版本信息(major和minor版本号)，一些标志,一个MethodDef token(定义了模块的入口)，可选的强名称数字签名.(关于CLR头的详细信息可查看CorHdr.h文件)
- 元数据:程序集描述文件.由定义表、引用表和清单表构成的二进制数据块.
![metadataDef](https://github.com/SixGodZhang/Materials/blob/master/Images/metadataDef.png)  
![medataref1](https://github.com/SixGodZhang/Materials/blob/master/Images/medataref1.png)  
![metadataref2](https://github.com/SixGodZhang/Materials/blob/master/Images/metadataref2.png)  
![mainfestMeta](https://github.com/SixGodZhang/Materials/blob/master/Images/mainfestMeta.png)  
- IL:Code

## 弱命名程序集和强命名程序集
弱命名程序集和强命名程序集的主要区别在于**强命名程序集采用了密钥对对程序集进行签名**.  
说明:  
这一对密钥允许对程序集进行唯一标识、保护和版本控制，并且允许将程序集部署到用户机器的任何地方，甚至是Internet上.  
私有部署和全局部署:  
**私有部署指部署到应用程序的基目录或子目录**，全局部署指部署到指定目录中.  
强命名程序集有四个重要特性,共同对程序集进行唯一标志:文件名(不包含扩展名)、版本号、语言文化和公钥(通常只取公钥的一部分值)  
使用SN.exe对程序集进行签名:  
生成公/私钥对:SN -k MyCompany.snk  
创建含有公钥的文件: SN -p MyCompany.snk MyCompany.PublicKey sha256  
控制台显示公钥:SN tp MyCompany.PublicKey  
![sn1](https://github.com/SixGodZhang/Materials/blob/master/Images/sn1.png) 
![sn2](https://github.com/SixGodZhang/Materials/blob/master/Images/sn2.png) 
![sn3](https://github.com/SixGodZhang/Materials/blob/master/Images/sn3.png) 

创建强命名程序集:  
csc /keyfile:MyCompany.snk Praogram.cs  
![key1](https://github.com/SixGodZhang/Materials/blob/master/Images/key1.png)

VS默认使用的是sha1计算的公钥，我们在命令行使用的是sha256计算的公钥，所以上图看到的公钥与我们命令行中的不同  
![sn4](https://github.com/SixGodZhang/Materials/blob/master/Images/sn4.png) 
这样，就一致了.  

如要在VS中改变计算公钥的方式.试验了下书中的方法,在AssemblyInfo.cs中增加如下代码:  
``` chsarp
[assembly: AssemblyAlgorithmId(System.Configuration.Assemblies.AssemblyHashAlgorithm.SHA256)]
```
编译代码之后，发现并没有效果，VS2017仍然使用的是sha1进行的签名.  
此部分正在验证中....有答案了，过来补充  

__程序集签名过程__
在对程序集进行签名时,会对整个PE文件进行hash处理,然后将此hash值用私钥进行签名，获取RSA签名,并将其存储在CLR头中.  
同时，公钥也被嵌入到了PE文件中的AssemblyDef清单元数据表中.  
![AssemblySinature](https://github.com/SixGodZhang/Materials/blob/master/Images/AssemblySinature.png) 


## 全局程序集缓存GAC
GAC(Global Assembly Cache),全局程序集缓存,将多个应用程序集访问的程序集放在公认目录，且CLR在加载的时候会检查该目录.  
GAC存储目录:  
%SystemRoot%Microsoft.NET/Assembly  
![gaclocation](https://github.com/SixGodZhang/Materials/blob/master/Images/gaclocation.png) 
将程序集安装到GAC目录下最常用的工具:GACUtil.exe、MSI  
GACUtil /i <assembly_path>  
GACUtil /u <assembly_display_name>  

引用强命名程序集时分两种,一种是在编译时引用，一种是在运行时引用:  
编译时，引用的强命名程序集是安装到编译器目录下的程序集.  
运行时，引用的强命名程序集是安装到GAC目录下的程序集.  

注意:  
 GAC目录下只能存放强命名程序集,且不能手动放置  

## CLR定位程序集
![clrloadassembly](https://github.com/SixGodZhang/Materials/blob/master/Images/clrloadassembly.png)
# 常用的的一些Bat命令

---
## 批处理快速查询
---
![批处理快速查询](https://www.jb51.net/help/cmd.htm)
### 命令列表
* [robocopy](#1)
* [echo](#2)
* [set](#3)
* [注释](#4)
* [call](#5)
* [if](#6)
* [pushd & popd](#7)
* [rd](#8)



<h2 id="1">robocopy </h2>
复制文件|文件夹 (仅限于Win)

``` bat
rem 将目录a中的文件全部copy到b目录,不包括目录
robocopy a b 
rem 将目录a下的所有文件夹&文件copy到b目录(包括空目录)
robocopy /e a b
``` 

<h2 id="2">echo </h2>
用法:  

``` bat
  ECHO [ON | OFF]
  ECHO [message]
```
示例:  

``` bat
rem 关闭回显
@echo off
rem 打开回显
@echo on
rem 输出信息
echo message

```

<h2 id="3">set </h2>
用法:  
``` bat
SET [variable=[string]]  
```
示例:
```
set root = C:\a
```

<h2 id="4">注释 </h2>
用法:  
``` bat
REM [comment]
:: [comment]
```
示例:
``` bat
rem 这是注释
:: [comment]
```

<h2 id="5">Call</h2>
用法:  
``` bat
CALL [drive:][path]filename [batch-parameters]

```
示例:
``` bat
call test.bat
```
<h2 id="6">if</h2>
用法:  
``` bat
rem 判断前一条命令的执行结果 0 success 1 fail
IF [NOT] ERRORLEVEL number command
rem 判断两个字符串的值是否相等
IF [NOT] string1==string2 command
rem 判断文件夹&文件是否存在
IF [NOT] EXIST filename command

```
示例:
``` bat
if exist C:\hello (
    echo hello
)
```

<h2 id="7">pushd & popd</h2>
用法:  
``` bat
rem 更改当前的执行目录
PUSHD [path | ..]
POPD
```

<h2 id="8">删除文件</h2>
用法:  
``` bat
rem 删除文件夹 /s 删除目录及其所有子目录和子文件 /q 安静模式，删除时不需要确认
RD [/S] [/Q] [drive:]path
```








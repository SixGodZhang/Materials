# Unity 代码加密(Linux)

## 背景描述
- 防止游戏被反编译，造成利益损害

## 思路概括
热更代码: hotfix code
主工程代码: trunk code = Assembly-CSharp.dll

一般做热更的游戏项目分为两部分代码,所以加密我们需要对这两部分加密.
其中,hotfix code 和 trunk code 可以分别用不同的加密的算法

主要就是先对hotfix code 进行加密,然后在trunk code 中进行解密,然后我们在把 trunk Code加密,在mono 中进行解密,这样一来，必须破解了mono才能看到
trunk code中对hotfix code的解密过程,最后才能去解密 hotfix code。这种 加密 方式,我相信一般的项目是够用了.

## 具体步骤

Linux Mono方面:
1. 环境搭建(包括反解APK的一套工具 + VM + Linux + Unity Mono源码)
2. 修改Mono中相关设置(采坑),确保能够编译出libmono.so
3. 修改Mono中相关加载代码的方法,进行解密操作
4. 在Mono中增加第三方的加密代码，达到加密目的

Window APK方面:
1. apktool 反解 apk 得到 资源及代码文件
2. 替换其中不同平台的libmono.so
3. 加密Assembly-CSahrp.dll
4. 打包，签名，测试


## Linux Mono 方面

### 环境搭建

1. VM 安装
官网下载,32、64 位均可,64位需要多安装两个工具( 过程略 )

2. Ubuntu 安装
官网下载即可

3. Ubuntu 下的工具安装
- autoconf
- automake
- bison
- git 
- libtool
- perl (安装NDK)
- libc6-dev-i386 (64位需要安装)
- lib32z1 (64位需要安装)
- libwww-perl (解决NDK安装过程中的问题)
- lib32stdc++6 (解决NDK安装过程中的问题)
- lib32ncurses5 (解决 c compiler 的问题)

ps: vim-gtk (方便查看和修改代码)

sudo vim /etc/vim/vimrc    修改vim配置
vim 配置:
set nu                           // 在左侧行号
set tabstop                  //tab 长度设置为 4
set nobackup               //覆盖文件时不备份
set cursorline               //突出显示当前行
set ruler                       //在右下角显示光标位置的状态行
set autoindent             //自动缩进


安装命令: sudo apt-get install <工具名称>

for example: 
- sudo apt-get install git 
- sudo apt-get install autoconf
- sudo apt-get install automake
- sudo apt-get install bison
- sudo apt-get install libtool
- sudo apt-get install libc6-dev-i386
- sudo apt-get install lib32z1
- sudo apt-get install libwww-perl
- sudo apt-get install lib32stdc++6
- sudo apt-get install lib32ncurses5
or:
- sudo apt-get install git autoconf automake bison libtool libc6-dev-i386 lib32z1 libwww-perl

注意:
libwww-perl 可以解决调用perl命令的问题,同时在第一次执行build_runtime_android.sh,可以自动安装NDK
lib32stdc++6 可以解决缺少libstdc++.so.6问题

下载Mono源码:
下载mono的压缩包(速度较快):
这里以Unity4.7.2作为参考,首先在Unity 在Github的官方账号上找到Mono项目:[Unity mono](https://github.com/Unity-Technologies/mono)
在分支上搜索Unity4.7,如果没有找到，则搜索前一个版本的分支，比如Unity4.6.原因: Unity mono如果没有做大的变动的话，依然使用的是前一个版本的mono
![搜索mono分支](https://github.com/SixGodZhang/Materials/blob/master/Images/findmonobranch.png)
直接下载mono unity-4.6分支的工程:
git clone -b unity-4.6 https://github.com/Unity-Technologies/mono.git

配置NDK环境:
把 mono/external/buildscripts 下的所有批处理脚本移动到mono目录下(注意: 不同分支的这些批处理脚本存放的位置可能不一样)
修改build_runtime_android.sh：
BUILDSCRIPTSDIR=external/buildscripts 因为我们将脚本移动到了mono目录下,所以删掉这个变量,并且与之相关的所有引用都修改一下


执行一次build_runtime_android.sh之后,
修改mono/external/android_krait_signal_handler/build.pl:
修改第一行#!/usr/bin/env perl -w为#!/usr/bin/perl -w (再不行 就删除 #!/usr/bin/env perl -w)
r16b 改为 r10e

注意:
configure: error: C compiler cannot create executables
android_krait_signal_handler 和 安装的gcc 版本不一致,缺少
sudo apt-get install lib32ncurses5 lib32z1

修改环境变量:sudo gedit ~/.bashrc
NDK_ROOT=/home/xxxx/xxxx/android-ndk-r10e   
NDK=$NDK_ROOT
ANDROID_NDK_ROOT=$NDK_ROOT
export NDK_ROOT NDK ANDROID_NDK_ROOT
使环境变量生效:source ~/.bashrc
测试: echo $ANDROID_NDK_ROOT

再次在mono目录下执行:build_runtime_android.sh
即可成功,在/builds/embedruntimes 下面会生成不同cpu架构的libmono.so

优化:
build_runtime_android.sh
注释151和152行,可以加快编译速度:
151 # clean_build "$CCFLAGS_ARMv5_CPU" "$LDFLAGS_ARMv5" "$OUTDIR/armv5"                                                                  
152 # clean_build "$CCFLAGS_ARMv6_VFP" "$LDFLAGS_ARMv5" "$OUTDIR/armv6_vfp"

编译release版本mono:
 -fpic -g -funwind-tables \  改为 -fpic -O2 -funwind-tables \ 

mono加密过程:
mono加载Assembly-CSharp.dll是在mono/mono/metadata/image.c中的mono_image_open_from_data_with_name中进行的
所以,我们的解密过程就在此处.
我引用的加密算法是xxtea,参考地址:[xxtea](https://github.com/xxtea/xxtea-dotnet)
1. 将xxtea.h 和 xxtea.c 放入mono/mono/metadata文件夹下,然后打开Makefile.am文件
2. 在libmonoruntime_la_SOURCES中添加xxtea.c ,在 libmonoruntimeinclude_HEADERS 中添加xxtea.h
3. 打开image.c,添加头文件引用: #include <mono/metadata/xxtea.h>
4. 
``` c
         if(name != NULL){
                 if(strstr(name,"Assembly-CSharp.dll")){
                          char *key = "XMX4GT8QBW1Z5GPZFNMHWLQ3PVM71G5FGID04DKD4IPZNQFYYB6BHWZC";
                          size_t len;
                          char* decryptData = (char *)xxtea_decrypt(data,data_len,key,&len);
                          int i = 0;
                          for(i=0;i<len;++i){
                                 data[i] = decryptData[i];
                          }
                          g_free(decryptData);
                         data_len = len;
                }
                 /*g_message("momo: %s",name);*/
         }
```

## Window APK 方面
Android 工具包:
APK 反编译、打包工具,签名工具....
下载地址:[Android工具包](https://github.com/SixGodZhang/AndroidTools)

DLL反编译工具:
下载地址:[dnspy](https://github.com/0xd4d/dnSpy)

加密过程:
首先拿到APK,然后对APK反解,在Managed文件夹下可以找到Assembly-CSharp.dll，然后用xxtea提供的接口对
Assembly-CSharp.dll进行加密,替换掉Managed文件夹下的；然后用编译出来的带解密功能的libmono.so文件
替换掉反解包中lib目录下的不同cpu架构的libmono.so文件.然后打包apk,进行签名即可


##参考博客
1. https://blog.csdn.net/KiTok/article/details/72472142
2. https://blog.csdn.net/qq_27772057/article/details/51945700
3. https://blog.csdn.net/u011643833/article/details/47261015
4. https://blog.csdn.net/u011535382/article/details/77924225
5. https://blog.csdn.net/qinglongyanyuezhu/article/details/52795308
6. https://blog.csdn.net/huutu/article/details/50829828

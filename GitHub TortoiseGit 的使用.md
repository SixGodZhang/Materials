# GitHub TortoiseGit的使用 #

## 1.配置私钥和公钥 ##
Toitoise使用的是ppk结尾的私钥文件，与Git生成的rsa文件结尾的不一样,所以需要重新配置

打开Tortoise的安装目录,找到以下两个EXE文件:

**puttygen:**用来生成私钥和公钥

**pageant:**为Tortoise保存私钥

## 2.具体步骤: ##
1. 用puttygen生成公钥，然后添加到github SSHkeys的管理列表中去
1. 用puttygen生成私钥,然后保存在本地某个目录下
1. 打开Tortoise的Setting->Git->Remote 依次进行配置，然后就可以进行相应操作了

## 3.参考资料: ##

[https://jingyan.baidu.com/article/63f236280f7e750209ab3d60.html](https://jingyan.baidu.com/article/63f236280f7e750209ab3d60.html "TortoiseGit的私钥和公钥生成")  TortoiseGit私钥和公钥生成


[https://www.cnblogs.com/lidabo/p/7457998.html](https://www.cnblogs.com/lidabo/p/7457998.html "TortoiseGit相关设置") TortoiseGit相关设置
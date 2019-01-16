# 常用 Git 命令清单

## 几个专用名词的译名如下:

Workspace：工作区  
Index / Stage：暂存区  
Repository：仓库区（或本地仓库）  
Remote：远程仓库  


## 一、新建代码库

__在当前目录新建一个Git代码库__  
$ git init

__新建一个目录，将其初始化为Git代码库__
$ git init [project-name]
 
__下载一个项目和它的整个代码历史__
$ git clone [url]


## 二、配置

Git的设置文件为 .gitconfig ，它可以在用户主目录下（全局配置），也可以在项目目录下（项目配置）。

__显示当前的Git配置__  
$ git config --list
 
__编辑Git配置文件__  
$ git config -e [--global]
 
__设置提交代码时的用户信息__  
$ git config [--global] user.name "[name]"
$ git config [--global] user.email "[email address]"


## 三、增加/删除文件

__添加指定文件到暂存区__  
$ git add [file1] [file2] ...
 
__添加指定目录到暂存区，包括子目录__  
$ git add [dir]
 
__添加当前目录的所有文件到暂存区__  
$ git add .
 
__添加每个变化前，都会要求确认__  
__对于同一个文件的多处变化，可以实现分次提交__  
$ git add -p
 
__删除工作区文件，并且将这次删除放入暂存区__  
$ git rm [file1] [file2] ...
 
__停止追踪指定文件，但该文件会保留在工作区__  
$ git rm --cached [file]
 
__改名文件，并且将这个改名放入暂存区__  
$ git mv [file-original] [file-renamed]



## 四、代码提交

__提交暂存区到仓库区__  
$ git commit -m [message]
 
__提交暂存区的指定文件到仓库区__  
$ git commit [file1] [file2] ... -m [message]
 
__提交工作区自上次commit之后的变化，直接到仓库区__  
$ git commit -a
 
__提交时显示所有diff信息__  
$ git commit -v
 
__使用一次新的commit，替代上一次提交__  
__如果代码没有任何新变化，则用来改写上一次commit的提交信息__  
$ git commit --amend -m [message]
 
__重做上一次commit，并包括指定文件的新变化__  
$ git commit --amend [file1] [file2] ...



## 五、分支

__列出所有本地分支__  
$ git branch
 
__列出所有远程分支__  
$ git branch -r
 
__列出所有本地分支和远程分支__  
$ git branch -a
 
__新建一个分支，但依然停留在当前分支__  
$ git branch [branch-name]
 
__新建一个分支，并切换到该分支__  
$ git checkout -b [branch]
 
__新建一个分支，指向指定commit__  
$ git branch [branch] [commit]
 
__新建一个分支，与指定的远程分支建立追踪关系__  
$ git branch --track [branch] [remote-branch]
 
__切换到指定分支，并更新工作区__  
$ git checkout [branch-name]
 
__切换到上一个分支__  
$ git checkout -
 
__建立追踪关系，在现有分支与指定的远程分支之间__  
$ git branch --set-upstream [branch] [remote-branch]
 
__合并指定分支到当前分支__  
$ git merge [branch]
 
__选择一个commit，合并进当前分支__  
$ git cherry-pick [commit]
 
__删除分支__  
$ git branch -d [branch-name]
 
__删除远程分支__  
$ git push origin --delete [branch-name]
$ git branch -dr [remote/branch]



## 六、标签

__列出所有tag__  
$ git tag
 
__新建一个tag在当前commit__  
$ git tag [tag]
 
__新建一个tag在指定commit__  
$ git tag [tag] [commit]
 
__删除本地tag__  
$ git tag -d [tag]
 
__删除远程tag__  
$ git push origin :refs/tags/[tagName]
 
__查看tag信息__  
$ git show [tag]
 
__提交指定tag__  
$ git push [remote] [tag]
 
__提交所有tag__  
$ git push [remote] --tags
 
__新建一个分支，指向某个tag__  
$ git checkout -b [branch] [tag]



## 七、查看信息

__显示有变更的文件__  
$ git status
 
__显示当前分支的版本历史__  
$ git log
 
__显示commit历史，以及每次commit发生变更的文件__  
$ git log --stat
 
__搜索提交历史，根据关键词__  
$ git log -S [keyword]
 
__显示某个commit之后的所有变动，每个commit占据一行__  
$ git log [tag] HEAD --pretty=format:%s
 
__显示某个commit之后的所有变动，其"提交说明"必须符合搜索条件__  
$ git log [tag] HEAD --grep feature
 
__显示某个文件的版本历史，包括文件改名__  
$ git log --follow [file]
$ git whatchanged [file]
 
__显示指定文件相关的每一次diff__  
$ git log -p [file]
 
__显示过去5次提交
$ git log -5 --pretty --oneline__  
 
__显示所有提交过的用户，按提交次数排序__  
$ git shortlog -sn
 
__显示指定文件是什么人在什么时间修改过__  
$ git blame [file]
 
__显示暂存区和工作区的差异__  
$ git diff
 
__显示暂存区和上一个commit的差异__  
$ git diff --cached [file]
 
__显示工作区与当前分支最新commit之间的差异__  
$ git diff HEAD
 
__显示两次提交之间的差异__  
$ git diff [first-branch]...[second-branch]
 
__显示今天你写了多少行代码__  
$ git diff --shortstat "@{0 day ago}"
 
__显示某次提交的元数据和内容变化__  
$ git show [commit]
 
__显示某次提交发生变化的文件__  
$ git show --name-only [commit]
 
__显示某次提交时，某个文件的内容__  
$ git show [commit]:[filename]
 
__显示当前分支的最近几次提交__  
$ git reflog



## 八、远程同步

__下载远程仓库的所有变动__  
$ git fetch [remote]
 
__显示所有远程仓库__  
$ git remote -v
 
__显示某个远程仓库的信息__  
$ git remote show [remote]
 
__增加一个新的远程仓库，并命名__  
$ git remote add [shortname] [url]
 
__取回远程仓库的变化，并与本地分支合并__  
$ git pull [remote] [branch]
 
__上传本地指定分支到远程仓库__  
$ git push [remote] [branch]
 
__强行推送当前分支到远程仓库，即使有冲突__  
$ git push [remote] --force
 
__推送所有分支到远程仓库__  
$ git push [remote] --all



## 九、撤销

__恢复暂存区的指定文件到工作区__  
$ git checkout [file]
 
__恢复某个commit的指定文件到暂存区和工作区__  
$ git checkout [commit] [file]
 
__恢复暂存区的所有文件到工作区__  
$ git checkout .
 
__重置暂存区的指定文件，与上一次commit保持一致，但工作区不变__  
$ git reset [file]
 
__重置暂存区与工作区，与上一次commit保持一致__  
$ git reset --hard
 
__重置当前分支的指针为指定commit，同时重置暂存区，但工作区不变__  
$ git reset [commit]
 
__重置当前分支的HEAD为指定commit，同时重置暂存区和工作区，与指定commit一致__  
$ git reset --hard [commit]
 
__重置当前HEAD为指定commit，但保持暂存区和工作区不变__  
$ git reset --keep [commit]
 
__新建一个commit，用来撤销指定commit__  
__后者的所有变化都将被前者抵消，并且应用到当前分支__  
$ git revert [commit]
 
__暂时将未提交的变化移除，稍后再移入__  
$ git stash
$ git stash pop



## 十、其他

__生成一个可供发布的压缩包__  
$ git archive
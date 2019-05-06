# 【CookBook】第一章学习笔记

## 目录
- [前言](#前言)
- [解压](#解压)
- [队列](#队列)
- [堆队列](#堆队列)

## 前言
《python3-CookBook》是基于python3的,有些语法并不适用于python2,但是其提供的大多数语法技巧还是值得的学习与参考的.  
第一章主要讲了数据结构和算法. 主要用到的python数据结构有元组、列表、字典,以及由这三种衍生出来的各种数据结构,算法主要
包含排序、查找、映射等

## 解压
这里的所谓解压,就是将一个数据结构中的数值取出来赋值给显示变量.解压分两种,一种是一对一解压,一种是一对多解压.
**一对一解压如下:**  
``` python
data = [ 'ACME', 50, 91.1, (2012, 12, 21) ]
name, shares, price, date = data

print(name)
print(shares)
print(price)
print(date)
``` 
**一对多解压如下:**
若参数用*表示,则会将传递进来的多个参数包装成元组  
若参数用**表示,则会将传递进来的命名参数包装成字典  
需要注意*表达式的顺序
``` python
def test_unzip_method(*tuple_data,**dict_data):
	print(tuple_data)
	print(dict_data)
	
test_unzip_method(1,2,3,4,name='zhang',id = 1223)

>Output:
(1, 2, 3, 4)
{'name': 'zhang', 'id': 1223}
``` 
## 队列
队列是一种先进先出(FIFO)的数据结构,主要介绍了在Python中关于collectiond.deque的用法:  
**定义:**
``` python
class collections.deque([iterable[, maxlen]]) 
```
**保留最后N个元素:**
``` python
from collections import deque

def search(lines, pattern, history=5):
    previous_lines = deque(maxlen=history)
    for line in lines:
        if pattern in line:
            yield line, previous_lines
        previous_lines.append(line)

# Example use on a file
if __name__ == '__main__':
    with open(r'../../cookbook/somefile.txt') as f:
        for line, prevlines in search(f, 'python', 5):
            for pline in prevlines:
                print("pre 5 lines: " + pline)
            print("line: " + line)
            print('-' * 20)
```

## 堆队列
堆队列,也称之为优先级队列.是一种基于数组和二叉树的数据结构.用法如下:
``` python
heap = []            # creates an empty heap
heappush(heap, item) # pushes a new item on the heap
item = heappop(heap) # pops the smallest item from the heap
item = heap[0]       # smallest item on the heap without popping it
heapify(x)           # transforms list into a heap, in-place, in linear time
item = heapreplace(heap, item) # pops and returns smallest item, and adds
                               # new item; the heap size is unchanged
```



















``` python
``` 


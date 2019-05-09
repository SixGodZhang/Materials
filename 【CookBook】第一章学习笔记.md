# 【CookBook】第一章学习笔记

## 目录
- [前言](#前言)
- [解压](#解压)
- [队列](#队列)
- [带优先级的元组](#带优先级的元组)
- [堆队列](#堆队列)
- [字典](#字典)
- [命名切片](#命名切片)
- [自定义类型的排序](#自定义类型的排序)
- [过滤序列元素](#过滤序列元素)

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

## 带优先级的元组
Python 中的元组可以指定优先级, 进而达到进行比较的目的.在Python 中,支持以下2中类型的优先级元组:
``` python
(priority, item)
(priority, index, item)
``` 
第一种元组,若遇到相同的优先级,则会报错, 而第二种可以避免这种情况的出现


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

**实现一个优先级队列**  
总是可以pop优先级最高的元素.
``` python
import heapq

class PriorityQueue:
    def __init__(self):
        self._queue = []
        self._index = 0

    def push(self, item, priority):
        heapq.heappush(self._queue, (-priority, self._index, item))
        self._index += 1

    def pop(self):
        return heapq.heappop(self._queue)[-1]
		
class Item:
    def __init__(self, name):
       self.name = name
    def __repr__(self):
       return 'Item({!r})'.format(self.name)

q = PriorityQueue()
q.push(Item('foo'), 1)
q.push(Item('bar'), 5)
q.push(Item('spam'), 4)

print(q.pop())
``` 

## 字典
主要介绍了Python中操作字典的一些惯用手法.
### 字典中的键映射多个值（deafultdict）
如果我们想做到一个键映射多个值,一般是将字典的多个值包装成一种数据结构即可.在Python中提供了collection.deafaultdict来供我们实现类似这种需求.
``` python
from collections import defaultdict

d = defaultdict(list)

d['a'].append(1)
d['a'].append(2)
d['b'].append(4)

print(d)

d = defaultdict(set)
d['a'].add(1)
d['a'].add(2)
d['b'].add(4)

print(d)
``` 
我们也可以换一种方式实现,但是可惜的是这种方式只可以在Python3中使用:
``` python
d = {} # 一个普通的字典
d.setdefault('a', []).append(1)
d.setdefault('a', []).append(2)
d.setdefault('b', []).append(4)

print(d)
``` 

### 字典排序(OrderedDict)
有时候我们会在迭代或者序列化的时候保持元素的顺序,此时就可以使用collections.OrderedDict  

**一般的dict的使用:**
在迭代的时候,取到的元素不一定是按顺序的.
``` python
d = {}

d['foo'] = 1
d['bar'] = 2
d['spam'] = 3
d['grok'] = 4
# Outputs "grok 4","foo 1", "bar 2", "spam 3"
for key in d:
    print(key, d[key])
``` 

**OrderedDict的使用:**
在迭代的时候,取到的元素的顺序与插入的顺序一致
``` python
from collections import OrderedDict

d = OrderedDict()
d['foo'] = 1
d['bar'] = 2
d['spam'] = 3
d['grok'] = 4
# Outputs "foo 1", "bar 2", "spam 3", "grok 4"
for key in d:
    print(key, d[key])
``` 

需要注意的是, OderedDict内部维护着一个双向链表,在存储大量数据时,需要考虑其内存消耗.


### 字典的运算
在字典上执行一些普通运算, 仅仅只会作用于键.此时,我们可以考虑其数学运算是否支持lamda表达式:
``` python
prices = {
    'ACME': 45.23,
    'AAPL': 612.78,
    'IBM': 205.55,
    'HPQ': 37.20,
    'FB': 10.75
}

print("min: " + min(prices))
print("max: " + max(prices))
#output: min: AAPL max: IBM

print("min: " + min(prices,key=lambda k:prices[k]))
print("max: " + max(prices,key=lambda k:prices[k]))
#output: min: FB max: AAPL

``` 

**排序:**
如果使用sorted的函数的话,需要注意的返回值时一个list.  
> 单纯对字典排序无意义,因为字典在迭代的时候本来就是一个无序的
``` python
prices = {
    'ACME': 45.23,
    'AAPL': 612.78,
    'IBM': 205.55,
    'HPQ': 37.20,
    'FB': 10.75
}

sdict = sorted(prices,key=lambda k:prices[k])
print("sdict: " + str(sdict))
``` 

也可以利用带有优先级的元组来达到排序的目的,稍微有点麻烦: 
``` python
prices = {
    'ACME': 45.23,
    'AAPL': 612.78,
    'IBM': 205.55,
    'HPQ': 37.20,
    'FB': 10.75
}

min_price = min(zip(prices.values(),prices.keys()))
print(min_price)
max_price = max(zip(prices.values(),prices.keys()))
print(max_price)
prices_sorted = sorted(zip(prices.values(),prices.keys()))
print(prices_sorted)
```  

如果待排序的是一个字典列表,那还可以通过使用operator模块的itemgetter函数来进行排序:
``` python
from operator import itemgetter

rows = [
    {'fname': 'Brian', 'lname': 'Jones', 'uid': 1003},
    {'fname': 'David', 'lname': 'Beazley', 'uid': 1002},
    {'fname': 'John', 'lname': 'Cleese', 'uid': 1001},
    {'fname': 'Big', 'lname': 'Jones', 'uid': 1004}
]

from operator import itemgetter
rows_by_fname = sorted(rows, key=itemgetter('fname'))
rows_by_uid = sorted(rows, key=itemgetter('uid'))
print(rows_by_fname)
print(rows_by_uid)
``` 

## 命名切片
在某些情景下，我们需要获取序列中某一个特定序列的内容,一般是直接将数据写死,也就是硬编码:
``` python
record = '....................100 .......513.25 ..........'
cost = int(record[20:23]) * float(record[31:37])

print(str(cost))
``` 
这种写法既不友好，也不美观. 为了解决这个问题, 我们可以采用Python内置的slice()函数:
``` python
SHARES = slice(20, 23)
PRICE = slice(31, 37)
cost = int(record[SHARES]) * float(record[PRICE])
``` 

## 自定义类型的排序
时常会遇到对自定义类型的排序操作,最常用的操作如下:
``` python
class User:
    def __init__(self, user_id):
        self.user_id = user_id

    def __repr__(self):
        return 'User({})'.format(self.user_id)
		
users = [User(23), User(3), User(99)]
print(users)
print(sorted(users, key=lambda u: u.user_id))
``` 

另外一种方式:
``` python
from operator import attrgetter

class User:
    def __init__(self, user_id):
        self.user_id = user_id

    def __repr__(self):
        return 'User({})'.format(self.user_id)
		
users = [User(23), User(3), User(99)]
print(users)
print(sorted(users, key=attrgetter('user_id')))


``` 

## 过滤序列元素
所谓过滤序列元素, 就是对一组数据进行筛选, 筛选出符合自己要求的元素. 有以下几种做法:

**普通做法:**
``` python
mylist = [1, 4, -5, 10, -7, 2, 3, -1]
print([n for n in mylist if n > 0])
``` 

**过滤器做法:**
``` python
mylist = [1, 4, -5, 10, -7, 2, 3, -1]

def meetMyNeed(val):
	return val > 0
	
print(list(filter(meetMyNeed,mylist)))
``` 

在过滤序列元素的时候,有一种特殊的情况:有两个序列,需要根据第二个序列来过滤第一个序列.简而言之,这两个序列是一个映射关系,
根据第二个序列的值是否符合要求来筛选第一个序列.  
具体做法就是根据第二个序列来创建筛子,然后去过滤第一个序列即可.
``` python
from itertools import compress

addresses = [
    '5412 N CLARK',
    '5148 N CLARK',
    '5800 E 58TH',
    '2122 N CLARK',
    '5645 N RAVENSWOOD',
    '1060 W ADDISON',
    '4801 N BROADWAY',
    '1039 W GRANVILLE',
]
counts = [ 0, 3, 10, 4, 1, 7, 6, 1]
more5 = [n > 5 for n in counts]

print(list(compress(addresses, more5)))
``` 


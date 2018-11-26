# 【CLR via C#】全书提炼总结-第二部分 类型设计

概要:
- [类型基础](#类型基础)
- [基元类型、引用类型和值类型](#基元类型、引用类型和值类型)
- [对象的相等性和同一性](#对象的相等性和同一性)
- [类型和成员基础](#类型和成员基础)
- [友元程序集](#友元程序集)
- [常量和字段](#常量和字段)
- [方法](#方法)
- [参数](#参数)
- [属性](#属性)
- [事件](#事件)
- [泛型](#泛型)
- [接口](#接口)

上一部分程序创建的过程:
![EXELoad](https://github.com/SixGodZhang/Materials/blob/master/Images/EXELoad.png)


## 类型基础
__CLR 在调用方法时加载类型的实例__:
1. CLR 在调用某个方法时,比如Main，第一次会通过JITCompiler将Main方法 编译成本机代码.
2. CLR会注意到Main中所有引用的类型的程序集是否都已经加载，然后根据元数据表，CLR为这些类型创建一个数据结构(如果该类型是第一次加载的话),
3. 创建数据结构的过程: 首先在堆上分配一块内存空间，将该类型的类型对象加载进来,(类型对象包含了类型对象指针，此处指向Type类型对象(所有类型对象都是Type类型的实例)、同步快索引、静态字段、方法的记录项等)
4. 在调用方法时，如果发现有创建类型实例Code，则再为类型的实例对象分配一块内存空间,该空间存储类型对象指针,指向类型对象、同步块索引、实例字段
![clrLoadClass](https://github.com/SixGodZhang/Materials/blob/master/Images/clrLoadClass.png)

## 基元类型、引用类型和值类型
编译器直接支持的数据类型称之为基元类型.
![primitivetype](https://github.com/SixGodZhang/Materials/blob/master/Images/primitivetype.png)
![csharptypes](https://github.com/SixGodZhang/Materials/blob/master/Images/csharptypes.png)
在值类型的使用过程中，需尽量避免装箱的性能消耗。

## 对象的相等性和同一性
相等性是指对象的内容相同,同一性指的是指向同一个对象.
Object中有Equals方法，默认实现的是对象的同一性,因为Object中有专门的方法实现了同一性ReferenceEquals，因此避免使用Object中的Equals方法来判断同一性,
通常,我们选择继承在子类中重写Equals来实现相等性.比如在ValueType中就重写了Equals方法来实现相等性.

在重写Equals方法之后，一定要重写GetHashCode方法,否则编译器将会给出警告.
关于对象的Hash值的用法:(以Dictionary为例)
当集合查找键时，先计算键的hash值，然后根据Hash判断键/值对存在哪个哈希桶中，然后在此哈希桶中以顺序遍历的方式找到该键值对.
在上述过程中，需要注意的就是Hash值的计算方式,它决定了键值对在哈希桶中分布的稠密程度.而Hash值的计算，正和我们上述提到的GetHashCode有关.

## 类型和成员基础
![classmembers](https://github.com/SixGodZhang/Materials/blob/master/Images/classmembers.png)

C# 中 成员的默认访问性
成员名称 | 默认的访问修饰符
--------|--------
class | internal
字段 | private
方法/属性 | private
接口中的方法 | public  abstract 

## 友元程序集
若A程序集想访问B程序集中的声明为internal的类型,即可以将A程序集声明为B程序集的的友元程序集，通过以下Code实现:
``` csharp
[assembly: InternalsVisibleTo("FriendAssemblyDemo_2")]
```
## 常量和字段
## 方法
## 参数
## 属性
## 事件
## 泛型
## 接口
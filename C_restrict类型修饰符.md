restrict 关键字允许编译器优化部分代码以更好地支持计算。 它只能用于指针, 表明该指针是访问数据对象的唯一且初始的方式.

restrict是一个很有意思的类型限定符, 以下属于个人理解:
restrict 是来约束程序员的, 并不是来约束编译器的
编译器会根据restrict 来尝试 优化代码, 但是并不一定会优化, 这取决于程序员的代码的操作
先理解一下书上的经典例子:
```c
int ar[10];
int * restrict restar = (int *) malloc (10 * sizeof(int))
int * par =ar;
```
这里的restrict 表明resstar 是访问一段内存的唯一且初始方式, 所以 编译器将会尝试优化使用restar的相关代码.
然后:
``` c
int n = 0;
for (n = 0; n<10;n++)
{
	par[n] += 5;
	restar[n] += 5;
	ar[n] *= 2;
	par[n] += 3;
	restar[n] +=3;
}
```
因为前面有restrict 声明, 所以编译器将会考虑对restar进行优化.

``` c
restar[n] += 5;
restar[n] += 3;
```

将被优化为:
restar[n] +=8;

优化结果如上. 但是, 编译器并不会尝试去优化par, 因为par不是唯一可以访问的数组的方式, 所以编译器没办法知道其他代码是否对这个地址的内容有修改， 导致优化之后结果不可预料。其实, 这也符合我们的直观理解.
拿svn打个比方, 可能不是特别恰当:
假设svn服务端的资源就是我们C编译器编译之后的C代码, 我们规定一个统一时间, 大家一起提交代码.
如果现在只有一个人从svn上拷贝出来了资源, 那这个人在怎么修改、优化资源，在最后规定时间提交到svn都是成功的。 因为没人和他冲突.
如果现在有2个及以上的人从svn上拷贝出来了资源, 各自在本地修改, 最后提交代码的时候, svn遇到冲突的地方是无法让这个提交成功的.
就如restrict一样, 如果有多个使用该地址的变量，则编译器不会去选择优化它, 也就是说, 让它们各自保留下来自己的操作, 如果只有一个使用该地址的变量, 那么我编译器就可以尝试优化了.
嗯嗯, 大概就是这样一个原理, 不知道有木有描述清楚, 反正我自己是清楚了~~

再举一个wiki上的例子吧:
有如下方法:

``` c
void updatePtrs(size_t *ptrA, size_t *ptrB, size_t *val)
{
  *ptrA += *val;
  *ptrB += *val;
}
```
编译器编译之后的代码类似于:
``` c
load R1 ← *val  ; Fetch from memory the value at address val
load R2 ← *ptrA ; Fetch from memory the value at address ptrA
add R2 += R1    ; Perform addition
store R2 → *ptrA  ; Update the value in memory location at ptrA
load R2 ← *ptrB ; 'load' may have to wait until preceding 'store' completes
load R1 ← *val  ; Have to load a second time to ensure consistency
add R2 += R1
store R2 → *ptrB
```
大概表现就是, val变量我们要从加载两次, 这种操作是可以被优化的， 优化之后的代码如下:
``` c
void updatePtrs(size_t *restrict ptrA, size_t *restrict ptrB, size_t *restrict val)
{
  *ptrA += *val;
  *ptrB += *val;
}
```
函数的类型限定符就多了restrict 修饰符.
编译之后的代码如下:
``` c
load R1 ← *val  ; Note that val is now only loaded once
load R2 ← *ptrA ; Also, all 'load's in the beginning ...
load R3 ← *ptrB
add R2 += R1
add R3 += R1
store R2 → *ptrA  ; ... all 'store's in the end.
store R3 → *ptrB
```
优化之后的表现就是, 加载两次val, 变成了加载一次val.这就是编译器对restrict类型修饰符的优化.
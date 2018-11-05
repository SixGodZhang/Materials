
//委托:
1.委托是不可变的，所以在多线程编程是可能会遇到如下问题:
一个委托已执行过非空条件判断，并且条件为真，然后在另一个线程中,委托被置为null，此时便会报错.
解决方案:
创建临时变量: Volatile.Read(ref 委托)

2.
合并委托
public static Delegate Combine(Delegate a, Delegate b);
public static Delegate Combine(params Delegate[] delegates);
移除
public static Delegate Remove(Delegate source, Delegate value);


3.遍历所有注册委托的方法
``` csharp
namespace DelegateDemo
{

    public delegate void OnClickPictureCallback(string msg);

    class Program
    {
        public static OnClickPictureCallback ClickPicture= null;

        static void Main(string[] args)
        {
            ClickPicture += OnClickT0;
            ClickPicture += OnClickT1;

            Delegate[] delegates = ClickPicture.GetInvocationList();
            foreach (Delegate item in delegates)
            {
                Console.WriteLine(item.Method);
                //item.DynamicInvoke("hello");
            }
        }

        public static void OnClickT0(string a)
        {
            Console.WriteLine("OnClickT0: " + a);
        }
        public static void OnClickT1(string a)
        {
            Console.WriteLine("OnClickT1: " + a);
        }

    }
}
```

4.显示实现的接口方法在IL中会被声明成:private virtual sealed,因此,只有接口的实例
才能访问该方法,且该方法不会被继承

5.忽略特殊符号比较字符串
``` csharp
        CompareInfo ci = CompareInfo.GetCompareInfo("zh-cn");
        int result = ci.Compare(a, b, CompareOptions.IgnoreSymbols);
        Console.WriteLine(result);
```
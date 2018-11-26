# C#中格式化字符串中的三大接口

IFormattable、IFormatProvider、ICustomFormatter


__IFormattable:__
``` csharp
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public interface IFormattable
    {
        string ToString(string format, IFormatProvider formatProvider);
    }
}
```

__IFormatProvider:__

``` csharp
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public interface IFormatProvider
    {
        object GetFormat(Type formatType);
    }
}
```

__ICustomFormatter:__
``` csharp
using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    public interface ICustomFormatter
    {
        string Format(string format, object arg, IFormatProvider formatProvider);
    }
}
```

eq:
``` csharp
string str = string.Format("{0},{1},{2}", "test", 112, DateTime.Now); 
```

执行过程:
string中的format最终会调到StringBuilder中AppendFormat方法.如果在string.Format中不指定IFormatProvider的实例对象的话,则会采用
当前线程上的Thread.CurrentThread.CurrentCulture作为默认值.在.NetFramework中有三个类实现了IFormatProvider接口,分别为CultureInfo、
NumberFormatInfo、DatetimeFormatInfo。

在调用到StringBuilder上的AppendFormat方法时，首先会调用IFormatProvider.GetFormat判断是否实现了ICustomFormatter，如果没有，则会调用
继承IFormattable的ToString方法，来达到格式化的目的.如果实现了ICustomFormatter,则会调用ICustomFormatter上的Format方法。


``` csharp
using System;
using System.Text;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(new BoldInt32s(),"{0} {1} {2}", "Jeffrey", 112, 12.22);
        Console.WriteLine(sb.ToString());
    }
}

internal class BoldInt32s : IFormatProvider, ICustomFormatter
{
    public object GetFormat(Type formatType)
    {
        if (formatType == typeof(ICustomFormatter))
            return this;
        return Thread.CurrentThread.CurrentCulture.GetFormat(formatType);
    }

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        string s;
        IFormattable formattable = arg as IFormattable;
        if (formattable == null) s = arg.ToString();
        else s = formattable.ToString(format, formatProvider);

        if (arg.GetType() == typeof(Int32))
            return "<B>" + s + "</B>";

        return s;

    }
}

```


``` csharp
 using System;
using System.Globalization;


public enum DaysOfWeek { Monday=1, Tuesday=2 };

public class TestFormatting
{
   public static void Main()
   {
      long acctNumber;
      double balance; 
      DaysOfWeek wday; 
      string output;

      acctNumber = 104254567890;
      balance = 16.34;
      wday = DaysOfWeek.Monday;


      output = String.Format(new AcctNumberFormat(), 
                             "On {2}, the balance of account {0:H} was {1:C2}.", 
                             acctNumber, balance, wday);
      Console.WriteLine(output);

      wday = DaysOfWeek.Tuesday;
      output = String.Format(new AcctNumberFormat(), 
                             "On {2}, the balance of account {0:I} was {1:C2}.", 
                             acctNumber, balance, wday);
      Console.WriteLine(output);
   }
}
// The example displays the following output:
//       On Monday, the balance of account 10425-456-7890 was $16.34.
//       On Tuesday, the balance of account 104254567890 was $16.34.

public class AcctNumberFormat : IFormatProvider, ICustomFormatter
{
    private const int ACCT_LENGTH = 12;

    public object GetFormat(Type formatType)
    {
        if (formatType == typeof(ICustomFormatter))
            return this;
        else
            return null;
    }

    public string Format(string fmt, object arg, IFormatProvider formatProvider)
    {
        // Provide default formatting if arg is not an Int64.
        if (arg.GetType() != typeof(Int64))
            try
            {
                return HandleOtherFormats(fmt, arg);
            }
            catch (FormatException e)
            {
                throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
            }

        // Provide default formatting for unsupported format strings.
        string ufmt = fmt.ToUpper(CultureInfo.InvariantCulture);
        if (!(ufmt == "H" || ufmt == "I"))
            try
            {
                return HandleOtherFormats(fmt, arg);
            }
            catch (FormatException e)
            {
                throw new FormatException(String.Format("The format of '{0}' is invalid.", fmt), e);
            }

        // Convert argument to a string.
        string result = arg.ToString();

        // If account number is less than 12 characters, pad with leading zeroes.
        if (result.Length < ACCT_LENGTH)
            result = result.PadLeft(ACCT_LENGTH, '0');
        // If account number is more than 12 characters, truncate to 12 characters.
        if (result.Length > ACCT_LENGTH)
            result = result.Substring(0, ACCT_LENGTH);

        if (ufmt == "I")                    // Integer-only format. 
            return result;
        // Add hyphens for H format specifier.
        else                                         // Hyphenated format.
            return result.Substring(0, 5) + "-" + result.Substring(5, 3) + "-" + result.Substring(8);
    }

    private string HandleOtherFormats(string format, object arg)
    {
        if (arg is IFormattable)
            return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
        else if (arg != null)
            return arg.ToString();
        else
            return String.Empty;
    }
}
```

__继承实现IFormattable接口，实现自定义格式化__
``` csharp
using System;
using System.Globalization;


internal class A:IFormattable
{
    public string name;
    public int id;

    public A(string name, int id)
    {
        this.name = name;
        this.id = id;
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        string result = null;

        string ft = format.ToUpper(CultureInfo.InvariantCulture);
        if (ft == "A")
            result = "{id: " + id + ", name: " + name + "}";
        else
            result = ToString();

        return result;
    }
}

class Program
{
    static void Main(string[] args)
    {
        A a = new A("zhangsan", 123);
        Console.WriteLine(string.Format("{0:A}",a));
        Console.WriteLine(string.Format("{0:B}",a));
    }
}

```
# 关于计算文件的MD5值的一点思考

MD5 Message-Digest Algorithm,首先，需要明白Md5的定位,md5是一种不可逆的算法,20世纪末因其被证明了不安全性，目前在计算机领域通常被应用于校验和.

使用方法:
``` csharp
byte[] MD5hash (byte[] data)
 {
    // This is one implementation of the abstract class MD5.
    MD5 md5 = new MD5CryptoServiceProvider();

    byte[] result = md5.ComputeHash(data);

    return result;
 }
```

关于MD5的C#的底层实现就不贴了,因为最后调用的也是C/C++代码.

在实际应用场景,通常会遇到Md5计算时间过长等问题.
在计算文件的Md5值问题上,
如果从文件上来说，分为大文件和小文件:
如果从性能上说,分为IO瓶颈和计算瓶颈

大文件上多半是一次性把文件全部读进了内存,这个问题其实与IO差不多,一般采用流来计算md5值即可

如果是计算上的瓶颈，可以采用多线程的方式来进行计算



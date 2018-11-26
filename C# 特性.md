# C# 特性

//
1.Serialiozation
``` csharp
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class TClass
{
    public int ID;
    public string Name;

    public TClass(int iD, string name)
    {
        ID = iD;
        Name = name;
    }

    public override string ToString()
    {
        return string.Format("id:{0},name:{1}", ID, Name);
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        TClass t = new TClass(1, "da");
        IFormatter format = new BinaryFormatter();

        //Serialize
        //Stream stream = new FileStream("TClass.bin", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        //format.Serialize(stream, t);
        //stream.Close();

        //Deserialize
        Stream readStream = new FileStream("TClass.bin", FileMode.Open, FileAccess.ReadWrite);
        TClass tt = (TClass)format.Deserialize(readStream);
        readStream.Close();
        Console.WriteLine(tt.ToString());
        //format.Serialize()

        Console.WriteLine("end...");
    }
}

```
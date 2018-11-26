# Database

//1.

``` csharp
using System;
using System.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        //new Program().CreateTable();
        //new Program().InsertTable();
        //new Program().SelectTable();
        new Program().DeleteFromTable();
    }

    public void DeleteFromTable()
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection("data source=.; database=student;integrated security=SSPI");
            string sql = "delete from student_info where id = 101";
            SqlCommand cm = new SqlCommand(sql, con);
            con.Open();
            cm.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            con.Close();
        }
    }

    public void SelectTable()
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection("data source=.; database=student;integrated security=SSPI");
            string sql = "select * from student_info";
            SqlCommand cm = new SqlCommand(sql, con);
            con.Open();
            SqlDataReader reader = cm.ExecuteReader();
            while (reader.Read())
            {
                //Console.WriteLine(reader["join_date"]);
                Console.WriteLine("[id:{0},name:{1},email:{2},join_date:{3}]", reader["id"], reader["name"], reader["email"], reader["join_date"]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: " + ex.Message + "\n" + ex.StackTrace);
        }
        finally
        {
            con.Close();
        }
    }

    /// <summary>
    /// 向表中插入数据
    /// </summary>
    public void InsertTable()
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection("data source=.; database=student;integrated security=SSPI");
            SqlCommand cm = new SqlCommand("insert into student_info(id,name,email,join_date) values('101','zhanghui','739536111@qq.com','2017-1-1')", con);
            con.Open();
            cm.ExecuteNonQuery();
            Console.WriteLine("数据录入成功!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: " + ex.Message);
        }
        finally
        {
            con.Close();
        }

        Console.WriteLine("end...");
    }

    /// <summary>
    /// 创建表
    /// </summary>
    public void CreateTable()
    {
        SqlConnection con = null;
        try
        {
            //data source=.; database=student; integrated security=SSPI
            con = new SqlConnection("data source=.;database=student;integrated security=SSPI");
            SqlCommand cm = new SqlCommand("create table student_info(id int not null,name varchar(100),email varchar(50),join_date date)", con);
            con.Open();
            cm.ExecuteNonQuery();
            Console.WriteLine("Table create successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("error: " + ex.Message);
        }
        finally
        {
            con.Close();
        }

        Console.WriteLine("end...");
    }
}
```
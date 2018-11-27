using SqlLite;
using System;
using System.IO;
using UnityEngine;

namespace Assets
{
    class TestSqlLite:MonoBehaviour
    {
        private void Start()
        {
            //创建SqLite数据库的实例
            SqlLiteCache cache = new SqlLiteCache();

            byte[] data = File.ReadAllBytes(Path.Combine(Application.dataPath, "earth.jpg"));

            ////向SqlLite中添加数据
            cache.Add(Guid.NewGuid().ToString(), new CacheItem()
            {
                Data = data,
            });

            //从SqlLite中取数据
            //CacheItem item = cache.Get(0 + "");
            //Debug.Log(item.AddedToCacheTicksUtc);

            //删除sqlLite中的数据
            //cache.Clear(0 + "");
            //Debug.Log("clear end");

            //修改sqlLite中的数据
        }
    }
}

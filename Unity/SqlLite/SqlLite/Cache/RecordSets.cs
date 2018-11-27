using SQLite4Unity3d;

namespace SqlLite
{
    /// <summary>
    /// PoCo类,sqlite.net 不支持多个primarykey
    /// </summary>
    public class RecordSets
    {
        [PrimaryKey,AutoIncrement]
        public int id { get; set; }//id 唯一标识

        public string name { get; set; }//名字

        public int timestamp { get; set; }//记录创建时间戳

        public byte[] data { get; set; }//数据

        public int? lastmodified { get; set; }//上一次修改时间
    }
}

using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SqlLite
{
    public class SqlLiteCache : ICache, IDisposable
    {
        private readonly uint _maxCacheSize;
        /// <summary>
        /// 每次像数据库中存储的记录项数目,避免频繁的IO操作
        /// </summary>
        private const int _pruneCacheDelta = 20;
        /// <summary>
        /// 计数器
        /// </summary>
        private int _pruneCacheCounter = 0;

        private bool _disposed;//资源是否被释放
        private string _dbName;
        private string _dbPath;
        private SQLiteConnection _sqlite;

        private object _lock = new object();

        public uint MaxCacheSize { get { return _maxCacheSize; } }
        public int PruneCacheDelta { get { return _pruneCacheDelta; } }


        public SqlLiteCache(uint? recordItemMax = null, string dbName = "cache.db")
        {
            _maxCacheSize = recordItemMax ?? 3000;
            _dbName = dbName;
            Init();
        }

        /// <summary>
        /// SqlLite 初始化
        /// </summary>
        private void Init()
        {
            OpenOrCreateDb(_dbName);

            List<SQLiteConnection.ColumnInfo> colInfos = _sqlite.GetTableInfo(typeof(RecordSets).Name);
            if (0 == colInfos.Count)
            {
                string cmdCreateTableSql = @"CREATE TABLE RecordSets(
id    INTEGER PRIMARY KEY ASC AUTOINCREMENT NOT NULL UNIQUE,
name  STRING  NOT NULL,
timestamp INTEGER NOT NULL,
data BLOB NOT NULL,
lastmodified INTEGER
);";
                _sqlite.Execute(cmdCreateTableSql);
                string cmdCreateIdxNamesSql = @"CREATE UNIQUE INDEX idx_names ON RecordSets (name ASC);";
                _sqlite.Execute(cmdCreateIdxNamesSql);
            }

            // some pragmas to speed things up a bit :-)
            // inserting 1,000 tiles takes 1-2 sec as opposed to ~20 sec
            string[] cmds = new string[]
            {
                "PRAGMA synchronous=OFF",
                "PRAGMA count_changes=OFF",
                "PRAGMA journal_mode=MEMORY",
                "PRAGMA temp_store=MEMORY"
            };
            foreach (var cmd in cmds)
            {
                try
                {
                    _sqlite.Execute(cmd);
                }
                catch (SQLiteException ex)
                {
                    // workaround for sqlite.net's exeception:
                    // https://stackoverflow.com/a/23839503
                    if (ex.Result != SQLite3.Result.Row)
                    {
                        UnityEngine.Debug.LogErrorFormat("{0}: {1}", cmd, ex);
                    }
                }
            }

        }

        /// <summary>
        /// 打开/创建一个数据库连接
        /// </summary>
        /// <param name="dbName"></param>
        private void OpenOrCreateDb(string dbName)
        {
            _dbPath = GetFullPath(dbName);
            _sqlite = new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.LogFormat("SQLLite Cache path----->{0}", _dbPath);
        }

        /// <summary>
        /// 获取数据库文件的完整路径(不同平台下路径不一样)
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        private string GetFullPath(string dbName)
        {
            string dbPath = Path.Combine(Application.persistentDataPath, "cache");
#if UNITY_EDITOT_WIN || UNITY_STANDALONE_WIN || UNITY_WSA
            dbPath = Path.GetFullPath(dbPath);
#endif
            if (!Directory.Exists(dbPath)) Directory.CreateDirectory(dbPath);
            dbPath = Path.Combine(dbPath, dbName);

            return dbPath;
        }

        /// <summary>
        /// 向SqlLite中添加数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cacheItem"></param>
        /// <param name="replaceIfExists"></param>
        public void Add(string name, CacheItem cacheItem, bool replaceIfExists = false)
        {

            //if data exist
            int? recordID = GetRecordID(name);
            if (recordID.HasValue && !replaceIfExists)
            {
                UnityEngine.Debug.LogFormat("id: {0} exist!",name);
                return;
            }

            //if data not exist
            lock (_lock)
            {
                //recordID = InsertRecord(name);
                int rowEffected = _sqlite.InsertOrReplace(new RecordSets
                {
                    name = name,
                    timestamp = (int)DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds,
                    data = cacheItem.Data,
                });

                if (1 != rowEffected)
                    throw new Exception("Insert data Failed!");
            }

            if (recordID < 0)
            {
                Debug.LogErrorFormat("{0} insert Failed ", name);
                return;
            }

            //检查溢出
            if (!replaceIfExists)
                _pruneCacheCounter++;

            if (0 == _pruneCacheCounter % _pruneCacheDelta)
            {
                _pruneCacheCounter = 0;
                Prune();
            }
        }

        /// <summary>
        /// 删除多余数据，保持数据在一个峰值(先进先出)
        /// </summary>
        private void Prune()
        {
            long count = _sqlite.ExecuteScalar<long>("SELECT COUNT(NAME) FROM RecordSets");
            if (count < _maxCacheSize) return;
            long toDelete = count - _maxCacheSize;
            try
            {
                // no 'ORDER BY' or 'LIMIT' possible if sqlite hasn't been compiled with 'SQLITE_ENABLE_UPDATE_DELETE_LIMIT'
                // https://sqlite.org/compile.html#enable_update_delete_limit
                // int rowsAffected = _sqlite.Execute("DELETE FROM tiles ORDER BY timestamp ASC LIMIT ?", toDelete);
                _sqlite.Execute("DELETE FROM RecordSets WHERE id IN ( SELECT id FROM RecordSets ORDER BY timestamp ASC LIMIT ? );", toDelete);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("error pruning: {0}", ex);
            }
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string id)
        {
            if (!RecordExistByID(id))
                return;
            _sqlite.Delete<RecordSets>(id);
        }

        /// <summary>
        /// 根据ID从SqlLite中取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CacheItem Get(string id)
        {
            RecordSets recordSets = null;
            if (!RecordExistByID(id))
                return null;

            recordSets = _sqlite
                .Table<RecordSets>()
                .Where(r => r.id.Equals(id))
                .FirstOrDefault();

            return new CacheItem()
            {
                Data = recordSets.data,
                AddedToCacheTicksUtc = recordSets.timestamp
            };
        }

        public void ReInit()
        {
            if (null != _sqlite)
            {
                _sqlite.Dispose();
                _sqlite = null;
            }

            Init();
        }

        private int InsertRecord(string recordName)
        {
            try
            {
                _sqlite.BeginTransaction(true);
                //return _sqlite.Insert(new RecordSets { name = recordName });
                RecordSets newRecordSets = new RecordSets { name = recordName };
                int rowEffected = _sqlite.Insert(newRecordSets);
                if (1!= rowEffected)
                {
                    throw new Exception("Insert Record Item Failed");
                }
                return newRecordSets.id;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("error:{0} cant insert", recordName);
                return -1;
            }
            finally {
                _sqlite.Commit();
            }
        }

        public bool RecordExistByID(string id)
        {
            RecordSets recordSets = _sqlite
                .Table<RecordSets>()
                .Where(ri => ri.id.Equals(id))
                .FirstOrDefault();
            return recordSets != null;
        }

        /// <summary>
        /// 根据名字获取record在表中的ID
        /// </summary>
        /// <param name="recordName"></param>
        /// <returns></returns>
        private int? GetRecordID(string recordName)
        {
            RecordSets recordSets = _sqlite
                .Table<RecordSets>()
                .Where(rn => rn.name.Equals(recordName))
                .FirstOrDefault();
            return recordSets == null ? (int?)null : recordSets.id;
        }

        ~SqlLiteCache()
        {
            Dispose(false);
        }

        private void Dispose(bool disposeManagedResources)
        {
            if (!_disposed)
            {
                if (disposeManagedResources && null == _sqlite)
                {
                    // compact db to keep file size small
                    _sqlite.Execute("VACUUM;");
                    _sqlite.Close();
                    _sqlite.Dispose();
                    _sqlite = null;
                }

                _disposed = true;
            }
            
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
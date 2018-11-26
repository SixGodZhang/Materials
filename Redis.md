#Redis

//1. 启动时遇到的问题
[6644] 02 Apr 23:11:58.976 # Creating Server TCP listening socket *:6379: bind: No such file or directory

解决方案如下按顺序输入如下命令就可以连接成功

1. redis-cli.exe
2. shutdown
3. exit
4. redis-server.exe redis.windows.conf

原因: 先启动了Redis-cli.exe,且未退出

//2.
启动客户端
redis-client.exe -h 127.0.0.1 -p 6379
 redis-cli -h host -p port -a password

//3.配置文件设置
config get * 获取所有配置
config set [name] [value] 设置配置属性
config get [name] 获取配置

//4.
Redis 命令

string 
redis 127.0.0.1:6379> SET name "runoob"
OK
redis 127.0.0.1:6379> GET name
"runoob"


//hash
redis> HMSET myhash field1 "Hello" field2 "World"
"OK"
redis> HGET myhash field1
"Hello"
redis> HGET myhash field2
"World"

//list
redis 127.0.0.1:6379> lpush runoob redis
(integer) 1
redis 127.0.0.1:6379> lpush runoob mongodb
(integer) 2
redis 127.0.0.1:6379> lpush runoob rabitmq
(integer) 3
redis 127.0.0.1:6379> lrange runoob 0 10
1) "rabitmq"
2) "mongodb"
3) "redis"
redis 127.0.0.1:6379>

//set
redis 127.0.0.1:6379> sadd runoob redis
(integer) 1
redis 127.0.0.1:6379> sadd runoob mongodb
(integer) 1
redis 127.0.0.1:6379> sadd runoob rabitmq
(integer) 1
redis 127.0.0.1:6379> sadd runoob rabitmq
(integer) 0
redis 127.0.0.1:6379> smembers runoob

//set
redis 127.0.0.1:6379> zadd runoob 0 redis
(integer) 1
redis 127.0.0.1:6379> zadd runoob 0 mongodb
(integer) 1
redis 127.0.0.1:6379> zadd runoob 0 rabitmq
(integer) 1
redis 127.0.0.1:6379> zadd runoob 0 rabitmq
(integer) 0
redis 127.0.0.1:6379> > ZRANGEBYSCORE runoob 0 1000
1) "mongodb"
2) "rabitmq"
3) "redis"

//
避免中文乱码
redis-cli --raw

//
redis 命令
	DEL key
    
该命令用于在 key 存在时删除 key。 被删除 key 的数量。

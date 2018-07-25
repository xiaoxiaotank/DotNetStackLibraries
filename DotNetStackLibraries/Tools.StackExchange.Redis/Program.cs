using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.StackExchange.Redis
{
    class Program
    {
        static void Main(string[] args)
        {
            Put();
            Get();
            Delete();
            Congfig();
            UseTransaction();


            Console.ReadKey();
        }

 
        private static void Put()
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                var db = redis.GetDatabase();

                #region 存储
                var value1 = "abcdefg";
                db.StringSet("mykey", value1);

                var key2 = new byte[] { 1, 2, 3, 4, 5, 6 };
                var value2 = new byte[] { 6, 5, 4, 3, 2, 1 };
                db.StringSet(key2, value2);
                #endregion

                #region pub / sub
                var sub = redis.GetSubscriber();
                sub.Subscribe("message", (channel, message) =>
                 {
                     Console.WriteLine((string)message);
                 });
                sub.Publish("message", "hello");
                #endregion

                #region 服务
                IServer server = redis.GetServer("localhost", 6379);
                //endpoints包含三部分:访问地址、传输协议、接口名
                var endPoints = redis.GetEndPoints();
                var lastSaveDt = server.LastSave();
                //var clients = server.ClientList();
                #endregion

                var key3 = db.KeyRandom();
                //db.StringIncrement(key3);
            }
        }

        private static void Get()
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                var db = redis.GetDatabase();

                #region 读取
                var value1 = db.StringGet("mykey");
                Console.WriteLine(value1);

                var key2 = new byte[] { 1, 2, 3, 4, 5, 6 };
                byte[] value2 = db.StringGet(key2);
                value2.ToList().ForEach(v => Console.Write($"{ v } "));
                Console.WriteLine();
                #endregion
            }
        }


        private static void Delete()
        {
            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                var db = redis.GetDatabase();

                #region 删除
                db.KeyDelete("mykey");
                //key不存在时，获取的值为0
                var value1 = db.StringGet("mykey");
                Console.WriteLine($"等于null吗? {value1.IsNull}") ;
                #endregion
            }
        }

        private static void Congfig()
        {
            var config = new ConfigurationOptions
            {
                EndPoints =
                {
                    {"redis0",6379 }
                },
                CommandMap = CommandMap.Create(new HashSet<string>
                {
                    "INFO", "CONFIG", "CLUSTER",
                    "PING", "ECHO", "CLIENT"
                }, false),
                KeepAlive = 180,
                DefaultVersion = new Version(2, 8, 8),
                Password = ""
            };

            using (var redis = ConnectionMultiplexer.Connect(config))
            {

            }
        }

        private static void UseTransaction()
        {
            using(var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                var db = redis.GetDatabase();
                var tran = db.CreateTransaction();
                var newId = new Guid().ToString();

                #region 方式一
                //添加事务前提：不存在{"key", "hashField"}这个对象
                //hashField为哈希字段，也就是哈希表中的key
                tran.AddCondition(Condition.HashNotExists("key", "hashField"));
                //将"hashField"修改为newId，或如果没有"key"，则直接创建一个持有哈希的并设置哈希的field为newId
                tran.HashSetAsync("key", "hashField", newId);
                bool isCommitted = tran.Execute();
                #endregion

                #region 方式二
                //Or 通过内置操作 When 来重写上面的实现方式
                bool wasSet = db.HashSet("key", "hashField", newId, When.NotExists); 
                #endregion
            }
        }

    }
}

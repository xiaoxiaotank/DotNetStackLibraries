using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoSQL.MongoDB
{
    class Program
    {
        static private readonly DbContext _ctx = new DbContext();

        static async Task Main(string[] args)
        {
            #region 基本使用
            //获取客户端
            var client = new MongoClient("mongodb://localhost:27017");
            //获取数据库
            var database = client.GetDatabase("foo");
            //获取集合
            var collection = database.GetCollection<BsonDocument>("bar");

            //创建bson对象
            var document = new BsonDocument()
            {
                ["name"] = "MongoDB",
                ["type"] = "Database",
                ["count"] = 1,
                ["info"] = new BsonDocument()
                {
                    ["x"] = 203,
                    ["y"] = 102
                }
            };

            //插入一个bson
            collection.InsertOne(document);
            //插入多个bson
            collection.InsertMany(new[] { document });

            //获取bson数据个数
            var count = collection.CountDocuments(new BsonDocument());
            //获取第一个bson数据
            var doc = collection.Find(new BsonDocument()).FirstOrDefault();
            //获取所有bson数据
            var docList = collection.Find(new BsonDocument()).ToList();
            //获取x=203的第一个bson数据
            var singleDoc = collection.Find(Builders<BsonDocument>.Filter.Eq("x", 203)).First();
            //获取x>200的所有数据光标
            var multiDocs = collection.Find(Builders<BsonDocument>.Filter.Gt("x", 200)).ToCursor();
            #endregion

            //插入省份
            var province = new Province("四川省");
            await _ctx.Provinces.InsertOneAsync(province);

            //更新省份，添加学校
            var school = new School("bq");
            province = await _ctx.Provinces.Find(p => p.Id == province.Id).FirstOrDefaultAsync();
            if(province != null)
            {
                province.Schools.Add(school);
                var result = await _ctx.Provinces.ReplaceOneAsync(p => p.Id == province.Id, province);
            }
            else
            {
                throw new Exception();
            }

            var provinceList = await _ctx.Provinces.Find(_ => true).ToListAsync();

            Console.WriteLine("Hello World!");
        }
    }
}

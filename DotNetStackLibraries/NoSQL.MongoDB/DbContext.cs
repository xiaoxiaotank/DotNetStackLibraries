using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoSQL.MongoDB
{
    class DbContext
    {
        private readonly IMongoDatabase _database;

        public IMongoCollection<Province> Provinces => _database.GetCollection<Province>(nameof(Province));

        public DbContext()
        {
            var client = new MongoClient("mongodb:\\locahost:27017");
            _database = client?.GetDatabase("dbname");
        }
    }
}

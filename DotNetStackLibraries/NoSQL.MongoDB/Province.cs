using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NoSQL.MongoDB
{
    class Province
    {
        [BsonId]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<School> Schools { get; set; }

        public Province(string name)
        {
            Name = name;
        }
    }
}

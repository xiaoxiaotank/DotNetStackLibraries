using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Tools.StackExchangeRedis
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var redis = ConnectionMultiplexer.Connect("localhost");

            var db = redis.GetDatabase();
            var value = "abcdefg";
            await db.StringSetAsync("mykey", value);

            Console.WriteLine(await db.StringGetAsync("mykey"));

            await redis.CloseAsync();

            Console.ReadKey();
        }
    }
}

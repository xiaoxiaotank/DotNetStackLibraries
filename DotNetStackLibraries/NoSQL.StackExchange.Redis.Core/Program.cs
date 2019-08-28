using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace NoSQL.StackExchange.Redis.Core
{
    class Program
    {
        private static readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect("localhost");
        private readonly IDatabase _db = _redis.GetDatabase();

        static async Task Main(string[] args)
        {
            
            Console.ReadKey();
        }

        /// <summary>
        /// 尝试获取用户
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TryGetUser(string token, out User user)
        {
            user = null;
            var value = _db.StringGet(token);
            if(value.HasValue)
            {
                user = SerializeHelper.Derialize<User>(value);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 添加或更新Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task AddOrUpdateTokenAsync(string token, User user, int? itemId)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            //缓存用户信息
            await _db.HashSetAsync(RedisKeys.Login, token, SerializeHelper.Serialize(user));
            //缓存最近请求信息
            await _db.SortedSetAddAsync(RedisKeys.Recent, token, timestamp);
            if(itemId.HasValue)
            {
                var key = $"{RedisKeys.ViewedItem}{token}";
                //缓存商品浏览信息
                await _db.SortedSetAddAsync(key, itemId, timestamp);
                //仅保留最新25条商品信息，清除过时信息
                await _db.SortedSetRemoveRangeByRankAsync(key, 0, -26);
                //记录商品的浏览次数，通过-1来让浏览次数多的商品位于集合前面
                await _db.SortedSetIncrementAsync(RedisKeys.ViewedItem, itemId, -1);
            }
        }

        private const int TokenLimitCount = 100000;
        /// <summary>
        /// 清理Token（守护进程）
        /// </summary>
        /// <returns></returns>
        public async Task CleanTokensAsync()
        {
            while (true)
            {
                var count = await _db.SortedSetLengthAsync(RedisKeys.Recent);
                if(count <= TokenLimitCount)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    continue;
                }

                var stopIndex = Math.Min(100, count - TokenLimitCount);
                var tokens = await _db.SortedSetRangeByRankAsync(RedisKeys.Recent, 0, stopIndex - 1);
                //清除用户信息
                await _db.HashDeleteAsync(RedisKeys.Login, tokens);
                //清除最近请求信息
                await _db.SortedSetRemoveAsync(RedisKeys.Recent, tokens);
                //清除商品浏览信息
                await _db.KeyDeleteAsync(tokens.Select(token => (RedisKey)$"{RedisKeys.ViewedItem}{token}").ToArray());
                //清除用户购物车信息
                await _db.KeyDeleteAsync(tokens.Select(token => (RedisKey)$"{RedisKeys.Cart}{token}").ToArray());
            }
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="token"></param>
        /// <param name="itemId"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        public async Task AddItemToCart(string token, int itemId, int itemCount)
        {
            await _db.HashSetAsync($"{RedisKeys.Cart}{token}", itemId, itemCount);
        }

        /// <summary>
        /// 从购物车移除商品
        /// </summary>
        /// <param name="token"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task RemoveItemFromCart(string token, int itemId)
        {
            await _db.HashDeleteAsync($"{RedisKeys.Cart}{token}", itemId);
        }

        /// <summary>
        /// 改变已访问商品的比例（守护进程）
        /// </summary>
        /// <returns></returns>
        public async Task RescaleViewedItem()
        {
            while (true)
            {
                //清除所有排名在20000名之外的商品
                await _db.SortedSetRemoveRangeByRankAsync(RedisKeys.ViewedItem, 0, -20001);
                //[可能需要优化]将浏览次数降低为之前的一半                
                foreach(var item in await _db.SortedSetRangeByRankWithScoresAsync(RedisKeys.ViewedItem))
                {
                    await _db.SortedSetAddAsync(RedisKeys.ViewedItem, item.Element, item.Score * 0.5);
                }

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        public async Task Publish()
        {
            await _db.PublishAsync("channel", "Hello!");
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <returns></returns>
        public async Task Subscribe()
        {
            var subs = _redis.GetSubscriber();
            await subs.SubscribeAsync("channel", (channel, value) =>
            {
                try
                {
                    Console.WriteLine(value);
                }
                catch { }
            });           
        }

        /// <summary>
        /// 对列表、集合和有序集合排序
        /// </summary>
        /// <returns></returns>
        public async Task Sort()
        {
            await _db.SortAsync(RedisKeys.ViewedItem);
        }

        /// <summary>
        /// 基本事务
        /// </summary>
        /// <returns></returns>
        public async Task BasicTransaction()
        {
            var trans = _db.CreateTransaction();
            await trans.StringIncrementAsync("number");
            //调用此句时才会执行事务之间的操作
            await trans.ExecuteAsync();
        }

        /// <summary>
        /// 过期
        /// </summary>
        /// <returns></returns>
        public async Task Expire()
        {
            var success = await _db.KeyExpireAsync("expire", TimeSpan.FromSeconds(10));
            var success1 = await _db.KeyExpireAsync("expire1", DateTime.Now.Add(TimeSpan.FromSeconds(10)));
            var ts = await _db.KeyTimeToLiveAsync("expire");
            success = await _db.KeyPersistAsync("expire");
        }
    }
}
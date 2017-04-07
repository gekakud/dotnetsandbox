using System;
using System.Collections.Generic;
using StackExchange.Redis;

namespace Redis
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cacheService = new RedisCache();

            Console.WriteLine("Cache is running:" + cacheService.IsCacheServiceAlive());
            if (cacheService.IsCacheServiceAlive())
            {
                cacheService.AddToList("names", new List<string>
                {
                    "vasya","kolya","petya"
                });
            }


            Console.ReadKey();
        }
    }

    public class RedisCache
    {
        private static ConfigurationOptions _sConfigOptions;

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection
            = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_sConfigOptions));

        public static ConnectionMultiplexer Connection => LazyConnection.Value;

        public RedisCache()
        {
            _sConfigOptions = new ConfigurationOptions();
            _sConfigOptions.EndPoints.Add("localhost:6379");
            _sConfigOptions.ClientName = "SafeRedisConnection";
            _sConfigOptions.ConnectTimeout = 100000;
            _sConfigOptions.SyncTimeout = 100000;
            _sConfigOptions.AbortOnConnectFail = false;
        }

        private static IDatabase Cache
        {
            get { return Connection.GetDatabase(); }
        }

        public bool IsCacheServiceAlive()
        {
            return Connection.IsConnected;
        }

        public void ClearItem(string p_key)
        {
            if (p_key == null)
            {
                throw new ArgumentNullException(nameof(p_key));
            }
            Cache.KeyDelete(p_key);
        }

        public void AddToList(string p_key, IEnumerable<string> p_list)
        {
            foreach (var u in p_list)
            {
                Cache.SetAdd(p_key, u, CommandFlags.FireAndForget);
            }
        }

        public bool Disconnect()
        {
            Connection.Close(true);
            return IsCacheServiceAlive();
        }
    }
}
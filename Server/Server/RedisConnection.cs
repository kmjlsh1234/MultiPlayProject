using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            var config = new ConfigurationOptions
            {
                EndPoints = { "127.0.0.1:6379" },
                AbortOnConnectFail = false,  // 연결 실패 시에도 재시도
                ConnectRetry = 3,            // 재시도 횟수
                ConnectTimeout = 5000,       // 연결 타임아웃(ms)
                KeepAlive = 180              // heartbeat (초)
            };
            return ConnectionMultiplexer.Connect(config);
        });

        public static ConnectionMultiplexer Connection => _lazyConnection.Value;

        public static IDatabase GetDatabase(int db = -1) => Connection.GetDatabase(db);
    }
}

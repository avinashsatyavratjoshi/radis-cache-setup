using RadisCache.Radis.Configuration;
using StackExchange.Redis;

namespace RadisCache.Radis.ConnectionWrapper
{
    public class RedisCacheConnectionWrapper : IRedisCacheConnectionWrapper
    {
        private readonly RedisCacheConfiguration _configuration;
        private readonly object _lock = new object();
        private volatile ConnectionMultiplexer _conn;
        private readonly Lazy<string> _connStr;
        public string KeyPrefix { get; set; }

        public RedisCacheConnectionWrapper(RedisCacheConfiguration configuration)
        {
            _configuration = configuration;
            _connStr = new Lazy<string>(GetRadisCacheConnectionString);
            KeyPrefix = configuration.KeyPrefix;
        }
        private string GetRadisCacheConnectionString()
        {
            return _configuration.ConnectionString;
        }
        private ConnectionMultiplexer GetRadisCacheConnection()
        {
            if(_conn != null && _conn.IsConnected)
            {
                return _conn;
            }
            lock(_lock)
            {
                try
                {
                    if (_conn != null && _conn.IsConnected)
                    {
                        return _conn;
                    }

                    _conn?.Dispose();

                    _conn = ConnectionMultiplexer.Connect(_connStr.Value);

                }
                catch (RedisConnectionException ex)
                {

                    return null;
                }
            }
          return _conn;
        }

        public IDatabase GetRadisCacheDB(int? db= null)
        {
            var connection = GetRadisCacheConnection();
            if (connection!=null)
            {
                return GetRadisCacheConnection().GetDatabase(db ?? -1);
            }
            else
            {
                return null;
            }
        }
        public void Dispose()
        {
            _conn?.Dispose();
        }

    }
}

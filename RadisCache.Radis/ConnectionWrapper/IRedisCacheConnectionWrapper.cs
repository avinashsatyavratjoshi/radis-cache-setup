using StackExchange.Redis;

namespace RadisCache.Radis.ConnectionWrapper
{
    public interface IRedisCacheConnectionWrapper : IDisposable
    {
        IDatabase GetRadisCacheDB(int? fb = null);
        string KeyPrefix { get; set; }
    }
}

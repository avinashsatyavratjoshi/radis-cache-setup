namespace RadisCache.Radis.Configuration
{
    public class RedisCacheConfiguration : IRedisCacheConfiguration
    {
        public string ConnectionString { get; set; }
        public string KeyPrefix { get; set; }
    }
}

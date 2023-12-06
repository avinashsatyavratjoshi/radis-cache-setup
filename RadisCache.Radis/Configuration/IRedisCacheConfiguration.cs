namespace RadisCache.Radis.Configuration
{
    public interface IRedisCacheConfiguration
    {
        public string ConnectionString { get;set; }
        public string KeyPrefix { get; set; }
    }
}

namespace RedisCache.Radis.RedisCacheOperations
{
    public interface IRedisCacheOperationsService
    {
        public bool IsRadisKeyFound(string key);
        public Task<T> GetCachedDataAsync<T>(string key);
        public Task<T> GetCachedDataAsync<T>(string key, Func<Task<T>> acquire);
        public Task SetCacheDataAsync(string key, object data);
        public Task RemoveCachedDataAsync(string key);
        public Task RemoveAndSetCacheDataAsync(string key, object data);
        public Task SetCacheDataWithExpiryDateAsync(string key, object data, DateTime expiryDate);
    }
}

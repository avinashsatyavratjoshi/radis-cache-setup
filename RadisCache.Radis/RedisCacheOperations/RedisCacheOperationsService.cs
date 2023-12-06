using Newtonsoft.Json;
using RadisCache.Radis.ConnectionWrapper;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisCache.Radis.RedisCacheOperations
{
    public class RedisCacheOperationsService : IRedisCacheOperationsService
    {

        private readonly IRedisCacheConnectionWrapper _connectionWrapper;

        private readonly IDatabase _radisCacheDb;

        public RedisCacheOperationsService(IRedisCacheConnectionWrapper connectionWrapper)
        {
            _connectionWrapper = connectionWrapper;
            _radisCacheDb = connectionWrapper.GetRadisCacheDB();
        }


        /// <summary>
        /// Method use to check whether or not specified key exists into the cache or not
        /// </summary>
        /// <param name="key">Key of cached item</param>
        public bool IsRadisKeyFound(string key)
        {
            if (_radisCacheDb == null)
                return false;

            return _radisCacheDb.KeyExists(key);

        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Key of cached item</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetCachedDataAsync<T>(string key)
        {
            if (_radisCacheDb == null || !IsRadisKeyFound(key))
                return default;

            //deserialize item
            var item = JsonConvert.DeserializeObject<T>(await _radisCacheDb.StringGetAsync(key));
            return item;
        }

        /// <summary>
        /// Get a cached item. If it's not in the cache yet, then load and cache it
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="acquire">Function to load item if it's not in the cache yet</param>
        /// <returns>The cached value associated with the specified key</returns>
        public async Task<T> GetCachedDataAsync<T>(string key, Func<Task<T>> acquire)
        {
            T result;

            if (_radisCacheDb != null)
            {
                //item already is in cache, so return it
                if (IsRadisKeyFound(key))
                    return await GetCachedDataAsync<T>(key);

                //or create it using passed function
                result = acquire != null ? await acquire() : default;

                //and set in cache (if cache time is defined)
                await SetCacheDataAsync(key, result);
            }
            else
            {
                //or create it using passed function
                result = acquire != null ? await acquire() : default;
            }

            return result;
        }

        /// <summary>
        /// Adds the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        public async Task SetCacheDataAsync(string key, object data)
        {
            if (data == null || _radisCacheDb == null)
                return;

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            await _radisCacheDb.StringSetAsync(key, serializedItem);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        public async Task RemoveCachedDataAsync(string key)
        {
            //remove item from caches
            await _radisCacheDb.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Removes the value with the specified key from the cache and add new value with specified key
        /// </summary>
        /// <param name="key">Key of cached item</param>
        public async Task RemoveAndSetCacheDataAsync(string key, object data)
        {
            //remove item from caches
            await _radisCacheDb.KeyDeleteAsync(key);

            await SetCacheDataAsync(key, data);
        }


        public async Task SetCacheDataWithExpiryDateAsync(string key, object data, DateTime expiryDate)
        {
            if (data == null || _radisCacheDb == null)
                return;

            //serialize item
            var serializedItem = JsonConvert.SerializeObject(data);

            //and set it to cache
            await _radisCacheDb.StringSetAsync(key, serializedItem);
            await _radisCacheDb.KeyExpireAsync(key, expiryDate);
        }

    }
}

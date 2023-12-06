using Microsoft.AspNetCore.Mvc;
using RadisCache.Radis;
using RedisCache.Radis.RedisCacheOperations;

namespace StackExchangeRadisCache.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        IRedisCacheOperationsService _redisCacheOperationsService;
        public HomeController(IRedisCacheOperationsService redisCacheOperationsService)
        {
            _redisCacheOperationsService = redisCacheOperationsService;
        }
        [HttpGet]
        public async Task<string> GetCachedValuesAsync()
        {
            var data = _redisCacheOperationsService.GetCachedDataAsync<string>("StoredCacheAPI");
            return data.Result;
        }
    }
}
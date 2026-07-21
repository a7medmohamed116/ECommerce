using ECommerce.Application.Contracts;
using ECommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<string?> GetDataAsync(string cacheKey, CancellationToken ct = default)
        
           => await _cacheRepository.GetAsync(cacheKey, ct);
        

        public async Task SetDataAsync(string cacheKey, object cacheValue, TimeSpan? TTL = null, CancellationToken ct = default)
        {
            var jasonValue = JsonSerializer.Serialize(cacheValue,new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            await _cacheRepository.SetAsync(cacheKey, jasonValue, TTL, ct);
        }
    }
}

using ECommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class CacheRepository : ICacheRepository
    {
        //connection => sql or redis ?
        private readonly IDatabase _database;
        public CacheRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase(); 
        }

        public async Task<string?> GetAsync(string cacheKey, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(cacheKey);
            return value.IsNullOrEmpty? null : value.ToString(); //back from jason so must convert to string 
        }

        public async Task SetAsync(string CacheKey, string cacheValue, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var result = await _database.StringSetAsync(CacheKey,cacheValue,timeToLive ?? TimeSpan.FromDays(2));
        }
    }
}

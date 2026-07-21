using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string cacheKey,CancellationToken ct =default);//cache key is the url (end point)
        Task SetAsync(string CacheKey, string cacheValue , TimeSpan?timeToLive = default ,CancellationToken ct =default);
    }
}

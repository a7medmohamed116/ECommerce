using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Basket;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        //Dbconnection -> redis // idistributedcache or  iconnectionmultiplexer in pack [stackExchange.redis]
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection) // regisert in di 
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null, CancellationToken ct = default)
        {
            //redis store only json data 
            var value = JsonSerializer.Serialize(basket);
            var result = await _database.StringSetAsync(basket.Id, value, TimeToLive ?? TimeSpan.FromDays(5));
            return result? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await _database.StringGetAsync(basketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket.ToString()); // get from json to app so must deserialize
        }
    }
}

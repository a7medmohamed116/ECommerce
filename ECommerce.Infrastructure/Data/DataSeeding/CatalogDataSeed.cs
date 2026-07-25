using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.Orders;
using ECommerce.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
//using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.Infrastructure.Data.DataSeeding
{
   
    public class CatalogDataSeed: IDataSeeder
    {
        private readonly StoreDbContext _dbContext;
        

        public CatalogDataSeed(StoreDbContext dbContext )
        {
            _dbContext = dbContext;
            
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                //check if there is any pending migrations
                var checkmigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (checkmigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                //D:\.NET Core\API\Projects\ECommerce.API\ECommerce.API\bin\Debug\net8.0\DataSeed
                var rootpath = Path.Combine(AppContext.BaseDirectory, "DataSeed");
                await SeedDataIfEmptyAsync<ProductType, int>(rootpath, "types.json", ct);
                await SeedDataIfEmptyAsync<ProductBrand, int>(rootpath, "brands.json", ct);
                await SeedDataIfEmptyAsync<Product, int>(rootpath, "products.json", ct);
                await SeedDataIfEmptyAsync<DeliveryMethod, int>(rootpath, "delivery.json", ct);

                var result = await _dbContext.SaveChangesAsync(ct);
                if (result > 0)
                {
                    //_logger.LogInformation($"Data Seeded Successfully {result} Rows Effected");
                    Console.WriteLine($"Data Seeded Successfully {result} Rows Effected");
                }
                else
                {
                    //_logger.LogInformation("Failed tio seed data");
                    Console.WriteLine("Failed tio seed data");
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                Console.WriteLine(ex.Message);
            }
          
        }

        // helper method to reed data from json fiels 
        private  async Task SeedDataIfEmptyAsync<T , TKey>(string rootpath , string FileName , CancellationToken ct) where T :BaseEntity<TKey>
        {
            if (await _dbContext.Set<T>().AnyAsync())
            {
                return;    
            }
            var filepath = Path.Combine(rootpath, FileName);
            if (!File.Exists(filepath))
            {
                return;
            }
            //unmanaged resource
            using var filestream = File.OpenRead(filepath);
            var items = await JsonSerializer.DeserializeAsync<List<T>>(filestream);
            if (items?.Any() ?? false)
                _dbContext.Set<T>().AddRange(items);
        }
    }
}

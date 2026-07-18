using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Data.DataSeeding;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure
{
    public static class InfrastructureServiceRegister
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            

            //services.AddScoped<IDataSeeder, CatalogDataSeed>();//issue
            //services.AddScoped<IDataSeeder, ÷identityDataSeed>();// مثلا ف وقتها هتعمل اوفريد وهتضرب ف لا اديها كيي//
            services.AddKeyedScoped<IDataSeeder, CatalogDataSeed>("Catalog");//issue
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>),typeof( GenericRepository<,>));
            //add redis database must be singleton run one 
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!); //get value from appsetting
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            

            return services;
        }
    }
}

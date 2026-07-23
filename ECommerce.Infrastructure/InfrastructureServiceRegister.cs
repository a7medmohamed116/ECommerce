using ECommerce.Application.Contracts;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Data.DataSeeding;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using ECommerce.Infrastructure.Identity.Services;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
        public static async Task<IServiceCollection> AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //var jwtSettings = new JwtSettings(); issue wrong => get new object with null values

            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });



            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });


            //services.AddScoped<IDataSeeder, CatalogDataSeed>();//issue
            //services.AddScoped<IDataSeeder, ÷identityDataSeed>();// مثلا 
            services.AddKeyedScoped<IDataSeeder, CatalogDataSeed>("Catalog");
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            //add redis database must be singleton run one 
            services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")!); //get value from appsetting
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICacheRepository, CacheRepository>();


            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();//

            services.AddScoped<IIdentityService, IdentityService>();/////
            services.AddScoped<ITokenService, TokenService>();

            var jwtSettings = configuration.GetSection("JWT").Get<JwtSettings>() ?? throw new InvalidOperationException("JWT Settings Error"); // get the data and put it class


            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // token successded
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; //request failed
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                //validations
                opt.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}

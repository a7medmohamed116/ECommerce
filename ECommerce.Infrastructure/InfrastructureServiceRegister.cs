using Azure.Core;
using ECommerce.Application.Common;
using ECommerce.Application.Contracts;
using ECommerce.Application.Services;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Data.DataSeeding;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using ECommerce.Infrastructure.Identity.Services;
using ECommerce.Infrastructure.Image;
using ECommerce.Infrastructure.Payments;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
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
            #region prosses of token with middleware and [Authorize]
            // decode the created token and check the data match or no then 
            //after check asp.net create object HttpContext.User kind of ClaimsPrincipal 
            //  ClaimsPrincipal
            //├── NameIdentifier = 15
            //├── Email = ahmed@gmail.com
            //├── Name = Ahmed
            //└── Role = Admin
            // in any controller can do var user = HttpContext.User; and reach data
            // why Authorize work 
            //    Request
            //     │
            //     ▼
            //JWT Middleware // decode the token and validate data
            //     │
            //  Token
            //     │
            //     ▼
            // ClaimsPrincipal
            //     │
            //     ▼
            //HttpContext.User
            //     │
            //     ▼
            //Authorize Attribute
            // if data in HttpContext.User go in if no 401 Unauthorized HttpContext.User if has role superadmin go in else 403 Forbidden
            // if [Authorize (Roles = "SuperAdmin")] check 
            #endregion



            services.AddSingleton<IPaymentGateway, StripePaymentGateway>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IRoleService, RoleService>();


            return services;
        }
    }
}

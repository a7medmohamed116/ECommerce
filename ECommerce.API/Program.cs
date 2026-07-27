
using ECommerce.Application;
using ECommerce.Application.Profiels;
using ECommerce.Infrastructure;
using ECommerce.Infrastructure.Identity.Services;

namespace ECommerce.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllers(); // api

            builder.Services.AddInfrastructureServices(builder.Configuration);//
            builder.Services.AddApplicationServices();//

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT")); //read this section and confert it as jwtsetting object register
            builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.SeedAndMigrateDataAsync();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

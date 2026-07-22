using ECommerce.Domain.Contracts;

namespace ECommerce.API
{
    public static class WebApplicationExtensions
    {
         public static async Task<WebApplication>SeedAndMigrateDataAsync (this WebApplication app)
         {
            using var scope = app.Services.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            var Identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");
            await seeder.SeedDataAsync();
            await Identityseeder.SeedDataAsync();
            return app;


         
         }
    }
}

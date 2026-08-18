using AdminDashBoard.Services;
using ECommerce.Application.Contracts;
using ECommerce.Application.Services;
using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Identity.Data;
using ECommerce.Infrastructure.Identity.Entities;
using ECommerce.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminDashBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();
            builder.Services
                    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Admin/Login";
                        options.AccessDeniedPath = "/Admin/AccessDenied";
                    });

            builder.Services.AddAuthorization();

            builder.Services.AddTransient<JwtAuthorizationHandler>();
            builder.Services.AddHttpClient<ProductApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7270/");
            }).AddHttpMessageHandler<JwtAuthorizationHandler>(); 
            builder.Services.AddHttpClient<AuthenticationApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7270/");
            });

        
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
           
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Admin}/{action=Login}/{id?}");

            app.Run();
        }
    }
}

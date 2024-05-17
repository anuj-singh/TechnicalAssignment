using InventoryManagement.DataAccess;
using InventoryManagement.Models;
using InventoryManagement.Repository.Implementations;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace InventoryManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
                                                options.UseSqlite(configuration.GetConnectionString("sqliteConn")));
            builder.Services.AddScoped<IExcelData,ExcelDataImport>();
            builder.Services.AddScoped<IDbProductOps,DbProductOperations>();
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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using WebApi.Logic.Services.Interfaces;
using WebApi.Logic.Services;
using WebApi.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CoffeeSharpDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IBalanceHistoryService, BalanceHistoryService>();
            builder.Services.AddScoped<IBalanceHistoryStatusService, BalanceHistoryStatusService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeSharp API", Version = "v1" });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

                var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeSharpDbContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                await ServiceSeeder.SeedAsync(serviceProvider, logger);
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeSharp API v1");
            });

            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.MapControllers();

            app.Run();
        }
    }
}
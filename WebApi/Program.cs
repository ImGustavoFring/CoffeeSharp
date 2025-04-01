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

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped<IClientCrudService, ClientCrudService>();
            builder.Services.AddScoped<IBalanceHistoryCrudService, BalanceHistoryCrudService>();
            builder.Services.AddScoped<IBalanceHistoryStatusCrudService, BalanceHistoryStatusCrudService>();
            builder.Services.AddScoped<IBranchCrudService, BranchCrudService>();
            builder.Services.AddScoped<IBranchMenuCrudService, BranchMenuCrudService>();
            builder.Services.AddScoped<ICategoryCrudService, CategoryCrudService>();
            builder.Services.AddScoped<IEmployeeCrudService, EmployeeCrudService>();
            builder.Services.AddScoped<IEmployeeRoleCrudService, EmployeeRoleCrudService>();
            builder.Services.AddScoped<IFeedbackCrudService, FeedbackCrudService>();
            builder.Services.AddScoped<IOrderCrudService, OrderCrudService>();
            builder.Services.AddScoped<IOrderItemCrudService, OrderItemCrudService>();
            builder.Services.AddScoped<IProductCrudService, ProductCrudService>();
            builder.Services.AddScoped<IRatingCrudService, RatingCrudService>();

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

                //await ServiceSeeder.SeedAsync(serviceProvider, logger); // forcing error so far
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
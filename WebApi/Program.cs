using Microsoft.EntityFrameworkCore;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using WebApi.Logic.CrudServices.Interfaces;
using WebApi.Logic.CrudServices;
using WebApi.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Logic.Services.Interfaces;
using WebApi.Logic.Services;
using System.Text.Json.Serialization;
using WebApi.Middleware;
using WebApi.Infrastructure.Repositories;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CoffeeSharpDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Регистрация CRUD-сервисов (оставляем без изменений)
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
            builder.Services.AddScoped<IAdminCrudService, AdminCrudService>();
            builder.Services.AddScoped<IMenuPresetCrudService, MenuPresetCrudService>();
            builder.Services.AddScoped<IMenuPresetItemCrudService, MenuPresetItemCrudService>();

            builder.Services.AddScoped<ServiceSeeder>(); // Temp

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProductCatalogService, ProductCatalogService>();

            // Регистрация Unit of Work для нового подхода
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeSharp API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireClaim("user_type", "admin"));

                options.AddPolicy("ChefOnly", policy =>
                    policy.RequireClaim("user_type", "chef"));
            });

            var app = builder.Build();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<CoffeeSharpDbContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                var seeder = serviceProvider.GetRequiredService<ServiceSeeder>(); // Temp
                await seeder.SeedAsync();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeSharp API v1");
            });

            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Run();
        }
    }
}

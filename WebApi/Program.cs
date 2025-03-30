using Microsoft.EntityFrameworkCore;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Microsoft.OpenApi.Models;
using WebApi.Logic.Services.Interfaces;
using WebApi.Logic.Services;
using WebApi.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Logic.Features.Interfaces;
using WebApi.Logic.Features;
using WebApi.Infrastructure.Middleware;

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

            builder.Services.AddScoped<IClientService, ClientService>();
            builder.Services.AddScoped<IBalanceHistoryService, BalanceHistoryService>();
            builder.Services.AddScoped<IBalanceHistoryStatusService, BalanceHistoryStatusService>();
            builder.Services.AddScoped<IBranchService, BranchService>();
            builder.Services.AddScoped<IBranchMenuService, BranchMenuService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IEmployeeRoleService, EmployeeRoleService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IMenuPresetService, MenuPresetService>();
            builder.Services.AddScoped<IMenuPresetItemService, MenuPresetItemService>();

            builder.Services.AddScoped<ServiceSeeder>(); //Temp

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();

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

                var dbContext = scope.ServiceProvider.GetRequiredService<CoffeeSharpDbContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();

                var seeder = serviceProvider.GetRequiredService<ServiceSeeder>(); //Temp
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
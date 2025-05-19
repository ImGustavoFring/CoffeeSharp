using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WebApi.Infrastructure.Data;
using WebApi.Logic.Services.Interfaces;
using WebApi.Logic.Services;
using WebApi.Infrastructure.Repositories.Interfaces;
using WebApi.Infrastructure.Repositories;
using WebApi.Infrastructure.UnitsOfWorks.Interfaces;
using WebApi.Infrastructure.UnitsOfWorks;
using WebApi.Middleware;
using CoffeeSharp.WebApi.Infrastructure.Data;
using Npgsql;
using System.Data;
using WebApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext();
});

var dbSettings = builder.Configuration
    .GetSection("ConnectionStrings")
    .Get<DatabaseSettings>();

builder.Services.AddDbContext<CoffeeSharpDbContext>(opt =>
    opt.UseNpgsql(dbSettings.DefaultConnection)
        .UseLazyLoadingProxies());

builder.Services.AddTransient<IDbConnection>(sp =>
    new NpgsqlConnection(dbSettings.LogDbConnection));

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ServiceSeeder>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));

builder.Services.Configure<TransactionSettings>(
    builder.Configuration.GetSection("Transaction"));

builder.Services.Configure<AuthorizationSettings>(
    builder.Configuration.GetSection("Authorization"));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("ConnectionStrings"));

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductCatalogService, ProductCatalogService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IReferenceDataService, ReferenceDataService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CoffeeSharp API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid JWT token."
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
            Array.Empty<string>()
        }
    });
});

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });

var authzSettings = builder.Configuration
    .GetSection("Authorization")
    .Get<AuthorizationSettings>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("user_type", "admin"));

    options.AddPolicy("ManagerOnly", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "user_type" && c.Value == "admin") ||
            (context.User.HasClaim(c => c.Type == "user_type" && c.Value == "employee") &&
             context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == authzSettings.ManagerRoleId))));

    options.AddPolicy("AllStaff", policy =>
        policy.RequireClaim("user_type", new[] { "admin", "employee" }));
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<CoffeeSharpDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();

    var seeder = services.GetRequiredService<ServiceSeeder>();
    await seeder.SeedAsync();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoffeeSharp API v1");
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

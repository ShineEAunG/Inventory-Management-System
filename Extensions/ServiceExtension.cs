using System.Text;
using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models.Authentication;
using InventoryManagementSystem.Models.Inventories;
using InventoryManagementSystem.Repository.FileRepo;
using InventoryManagementSystem.Repository.Implementations;
using InventoryManagementSystem.Repository.Interface;
using InventoryManagementSystem.Repository.Interfaces;
using InventoryManagementSystem.Services.FileHandling;
using InventoryManagementSystem.Services.Implementations;
using InventoryManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;

namespace InventoryManagementSystem.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection DbConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new NullReferenceException("Connection String to db is not set in there");
        }
        services.AddDbContextPool<AppDbContext>(options => options.UseNpgsql(connectionString));
        services.AddDbContextPool<AppDbContext>(options =>
        options.UseNpgsql(connectionString, npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(60); // Optional: set command timeout
        }));
        return services;
    }

    public static void CorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
         options.AddPolicy("AllowAny", policy =>
        {
            policy.AllowAnyOrigin()
                // .WithOrigins("http://Example.com:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
   
        }));
    }
    public static void ServiceCollectionConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IGenericRepo<Employee>, GenericRepo<Employee>>();
        services.AddSingleton<SupaBaseService>();
        services.AddScoped<IGenericRepo<Item>, GenericRepo<Item>>();
        services.AddScoped<IFileMetaDataRepo, FileMetaDataRepo>();
        services.AddScoped<IItemRepo, ItemRepo>();
        services.AddScoped<IGenericRepo<Category>, GenericRepo<Category>>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IEmployeeRoleRepo, EmployeeRoleRepo>();
        services.AddSingleton<IFileService, FileService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IEmployeeRepo, EmployeeRepo>();
        services.AddScoped<IRefreshTokenRepo, RefreshTokenRepo>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddHttpContextAccessor();
    }
    public static void JwtSchemeConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(options =>
            {
                // default scheme is cookies, so here we change it to jwt bearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; // this only here is default
            }
        ).JwtBearerConfiguration(config);   //change to cookies or other scheme if needed
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanDelete", policy =>
                policy.RequireClaim("permissions", "Can Delete"));
            options.AddPolicy("CanAdd", policy =>
                policy.RequireClaim("permissions", "Can Add"));
            options.AddPolicy("CanEdit", policy =>
                policy.RequireClaim("permissions", "Can Edit"));
        });
    }
    public static AuthenticationBuilder JwtBearerConfiguration(this AuthenticationBuilder authBuilder, IConfiguration config)
    {
        var issure = config["JwtSetting:Issuer"];
        var audience = config["JwtSetting:Audience"];
        var secret = config["JwtSetting:Secret"];

        if (secret is null || audience is null || issure is null)
            throw new ApplicationException("JwtSetting is not configured");

        var issureSingingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        authBuilder.AddJwtBearer(options =>
            {
                //options.RequireHttpsMetadata = false;  // only for development testing 
                // in production it is set to be true;
                options.SaveToken = true; // defalut is false if true save token in authentication properties and we can retrieve it later in service or controller
                // the line above is for this codes var token = await HttpContext.GetTokenAsync("access_token");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidIssuer = issure,
                    IssuerSigningKey = issureSingingKey
                };
            }
        );
        return authBuilder;
    }
}
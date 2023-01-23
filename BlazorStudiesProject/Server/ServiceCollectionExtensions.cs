using BlazorStudiesProject.Server.Data;
using BlazorStudiesProject.Server.Entities;
using BlazorStudiesProject.Server.Repositories;
using BlazorStudiesProject.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddGoogle(o =>
        {
            o.ClientId = "xxx"; //put yours credentials 
            o.ClientSecret = "xxx"; //put yours credentials
            o.SignInScheme = "Identity.Application";
        });

        return services;
    }
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services
            .AddSingleton<AppBackgroundWorker>()
            .AddSingleton<TestService>()
            .AddScoped<IBooksRepository, BooksRepository>()
            .AddScoped<IBookUserInfosRepository, BookUserInfosRepository>()
            .AddScoped<IAdditionalInfoRepository, AdditionalInfoRepository>()
            .AddScoped<IPurchasesRepository, PurchasesRepository>();

        return services;
    }

    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services
        .AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Filename=data.db"));
        return services;
    }

    public static IServiceCollection RegisterIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(o => { })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 1;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 2;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.RequireUniqueEmail = false;
        });

        return services;
    }
}

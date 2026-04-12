using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;
using Shared.Data.Interceptors;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add services to the container.

        // Add Endpoint services

        // Add Application use case services
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Add validators from Assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Add Data - Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<CatalogDBContext>((sp, options) => 
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDataSeeder, CatalogDataSeeder>();

        return services;
    }

    public static async Task<IApplicationBuilder> UseCatalogModule(
        this IApplicationBuilder app) 
    {
        // Configure the HTTP request pipeline.
        
        // 1. Use Api Endpoint services

        // 2. Use application use case services

        // 3. Use Data - Infrastructure services
        await app.UseMigration<CatalogDBContext>();

        return app;
    }
}

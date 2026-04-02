using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        // Add Data - Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<CatalogDBContext>(options =>
            options.UseNpgsql(connectionString));

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

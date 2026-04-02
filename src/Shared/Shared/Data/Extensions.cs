using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;

namespace Shared.Data;

public static class Extensions
{
    public static async Task<IApplicationBuilder> UseMigration<TContext>(this IApplicationBuilder app)
        where TContext : DbContext
    {
        await MigrateDatabaseAsync<TContext>(app.ApplicationServices);

        await SeedDataAsync(app.ApplicationServices);

        return app;
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider serviceProvider) 
        where TContext : DbContext
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        await context.Database.MigrateAsync();
    }

    private static async Task SeedDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IDataSeeder>();

        foreach (var seeder in seeders)
        {
            await seeder.SeedAllAsync();
        }

    }
}

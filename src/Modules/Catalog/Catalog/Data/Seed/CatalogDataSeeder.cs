namespace Catalog.Data.Seed;

public class CatalogDataSeeder(CatalogDBContext dBContext)
    : IDataSeeder
{
    public async Task SeedAllAsync()
    {
        if (!await dBContext.Products.AnyAsync())
        {
            await dBContext.Products.AddRangeAsync(InitialData.Products);
            await dBContext.SaveChangesAsync();
        }
    }
}

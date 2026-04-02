namespace Catalog.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Category).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(200);
        builder.Property(p => p.ImageFile).HasMaxLength(100);
        //builder.Property(p => p.Price).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.Price).IsRequired();
    }
}

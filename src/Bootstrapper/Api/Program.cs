var builder = WebApplication.CreateBuilder(args);

// Add services to the container

builder.Services.AddCarterWithAssemblies(
    typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);


var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapCarter();
await app.UseCatalogModule();
await app.UseBasketModule();
await app.UseOrderingModule();

app.Run();
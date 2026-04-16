var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container

builder.Services.AddCarterWithAssemblies(
    typeof(CatalogModule).Assembly);

builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();


// Build the app -------------------
var app = builder.Build();
// ---------------------------------


// Configure the HTTP request pipeline.

app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });

await app.UseCatalogModule();
await app.UseBasketModule();
await app.UseOrderingModule();

app.Run(); 
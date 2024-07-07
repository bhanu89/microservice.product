using microservice.product.Application.CommandHandlers;
using microservice.product.Application.Commands;
using microservice.product.Application.Queries;
using microservice.product.Application.QueryHandlers;
using microservice.product.Data;
using microservice.product.Data.Cache;
using microservice.product.Data.Entities;
using microservice.product.Data.Repositories;
using microservice.product.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Singletons
builder.Services.AddSingleton<MemCacheRepository<ProductEntity>>();
builder.Services.AddSingleton<MemCacheRepository<CurrencyEntity>>();
builder.Services.AddSingleton<DatabaseContext>();

// Scoped classes
builder.Services.AddScoped(typeof(IRepository<ProductEntity>), typeof(ProductEntityRepository));
builder.Services.AddScoped(typeof(IRepository<CurrencyEntity>), typeof(CurrencyEntityRepository));
builder.Services.AddScoped<AddProductCommandHandler>();
builder.Services.AddScoped<GetProdcutsQueryHandler>();
builder.Services.AddScoped<UpdateProductCommandHandler>();
builder.Services.AddScoped<DeleteProductCommandHandler>();

// Transient classes
builder.Services.AddTransient<GetProductsQuery>();
builder.Services.AddTransient<AddProductCommand>();
builder.Services.AddTransient<UpdateProductCommand>();
builder.Services.AddTransient<DeleteProductCommand>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

var app = builder.Build();

// custom middleware
app.UseMiddleware<SlidingRateLimitMiddleware>(5, TimeSpan.FromMinutes(1)); // limit at 5 requests within the last minute

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

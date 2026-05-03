using CatalogService.DbContexts;
using CatalogService.Repositories;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.DbContexts;
using WMSCommon.Extensions;
using WMSCommon.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppSettingsConfig();
builder.Services.AddFrontendCORS();
builder.Host.AddSerilog();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<ITenantRepository<IProductDetail>>(sp => 
    new ProductDetailRepository(
        sp.GetRequiredService<CatalogDbContext>(),
        sp.GetRequiredService<IUserContext>()
    ));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFrontendAuthentication(builder.Configuration);
builder.Services.AddMessageBus<CatalogDbContext>(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.SetupMiddleware();
await app.ApplyMigrations<CatalogDbContext>("Catalog Service");

app.Run();

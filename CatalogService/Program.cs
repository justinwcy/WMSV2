using CatalogService.DbContexts;
using CatalogService.Models;
using CatalogService.Repositories;
using CatalogService.Services;
using Microsoft.EntityFrameworkCore;
using WMSCommon.Contexts;
using WMSCommon.Contracts.CatalogService;
using WMSCommon.DbContexts;
using WMSCommon.Extensions;
using WMSCommon.Repositories;
using WMSCommon.Services;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppSettingsConfig();
builder.Services.AddFrontendCORS();
builder.Host.AddSerilog();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IGenericSyncService<IProductDetail>, ProductDetailService>();
builder.Services.AddScoped<IGenericRepository<ProductDetail>, GenericRepository<ProductDetail, CatalogDbContext>>();

builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFrontendAuthentication(builder.Configuration);
builder.Services.AddMessageBus<CatalogDbContext>(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.SetupMiddleware();
await app.ApplyMigrations<CatalogDbContext>("Catalog Service");

app.Run();

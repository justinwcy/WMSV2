using CatalogService.DbContexts;
using CatalogService.Models;
using CatalogService.Repositories;
using CatalogService.Services;
using WMSCommon.Contexts;
using WMSCommon.DbContexts;
using WMSCommon.Extensions;
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

builder.Services.AddAppDbContextFactory<CatalogDbContext>(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddFrontendAuthentication(builder.Configuration);
builder.Services.AddMessageBus<CatalogDbContext>(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.SetupMiddleware();
await app.ApplyMigrations<CatalogDbContext>("Catalog Service");

app.Run();

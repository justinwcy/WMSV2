using System.Text.Json.Serialization;
using SalesService.DbContexts;
using SalesService.Repositories;
using WMSCommon.Contexts;

using WMSCommon.DbContexts;
using WMSCommon.Extensions;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppSettingsConfig();
builder.Services.AddFrontendCORS();
builder.Host.AddSerilog();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();
builder.Services.AddFrontendAuthentication(builder.Configuration);
builder.Services.AddMessageBus<SalesDbContext>(builder.Configuration, opts =>
{
    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);
    opts.ListenToRabbitQueue("product-detail-sync");
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.SetupMiddleware();
await app.ApplyMigrations<SalesDbContext>("Sales Service");


app.Run();
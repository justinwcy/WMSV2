using System.Text.Json.Serialization;
using FacilityService.DbContexts;
using WMSCommon.Contexts;

using WMSCommon.DbContexts;
using WMSCommon.Extensions;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppSettingsConfig();
builder.Services.AddFrontendCORS();
builder.Host.AddSerilog();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters
            .Add(new JsonStringEnumConverter());
    });
builder.Services.AddOpenApi();
builder.Services.AddFrontendAuthentication(builder.Configuration);
builder.Services.AddMessageBus<FacilityDbContext>(builder.Configuration, opts =>
{
    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);
    opts.ListenToRabbitQueue("staff-sync");
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.SetupMiddleware();
await app.ApplyMigrations<FacilityDbContext>("Facility Service");


app.Run();
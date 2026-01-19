using Microsoft.EntityFrameworkCore;
using OrganizationService.Contexts;
using OrganizationService.DbContexts;
using WMSCommon.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<OrganizationDbContext>(
    (serviceProvider, options) => {
        var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();

        options.UseSqlServer(connectionString)
            .AddInterceptors(auditInterceptor);
    },
    ServiceLifetime.Scoped);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using OrganizationService.DbContexts;
using OrganizationService.Models;
using OrganizationService.Repositories;
using OrganizationService.Service;

using WMSCommon.Contexts;
using WMSCommon.DbContexts;
using WMSCommon.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddAppSettingsConfig();
builder.Services.AddFrontendCORS();
builder.Host.AddSerilog();
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddAppDbContextFactory<OrganizationDbContext>(builder.Configuration);

// For Identity, MassTransit, and Scoped Services
builder.Services.AddAppDbContext<OrganizationDbContext>(builder.Configuration);

builder.Services.AddIdentity<Staff, IdentityRole<Guid>>(
        options =>
        {
            options.Password.RequiredLength = 6;
        })
    .AddEntityFrameworkStores<OrganizationDbContext>();

builder.Services.AddOpenApi();

builder.Services.AddFrontendAuthentication(builder.Configuration);
builder.Services.AddMessageBus<OrganizationDbContext>(builder.Configuration);
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.SetupMiddleware();
await app.ApplyMigrations<OrganizationDbContext>("Organization Service");

app.Run();

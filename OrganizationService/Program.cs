using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using OrganizationService.Contexts;
using OrganizationService.DbContexts;
using OrganizationService.Models;
using OrganizationService.Repositories;
using OrganizationService.Service;

using Serilog;

using WMSCommon.Constants;
using WMSCommon.Contexts;

var builder = WebApplication.CreateBuilder(args);

var contentRootPath = builder.Environment.ContentRootPath;
var parentPath = Directory.GetParent(contentRootPath);
string wmsCommonPath = Path.Combine(parentPath.FullName, "WMSCommon");
// Load the production configuration first
builder.Configuration.AddJsonFile(
    Path.Combine(wmsCommonPath, "common_appsettings.json"),
    optional: true,
    reloadOnChange: true);

// Then, load the local appsettings.json.
// This allows local settings to override shared ones.
builder.Configuration.AddJsonFile(
    "appsettings.json",
    optional: false,
    reloadOnChange: true);

// load the development configuration to override it
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile(
        Path.Combine(wmsCommonPath, "common_appsettings.Development.json"),
        optional: true,
        reloadOnChange: true);

    builder.Configuration.AddJsonFile(
        "appsettings.Development.json",
        optional: false,
        reloadOnChange: true);
}

// add CORS

var devPolicyName = "DevPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(devPolicyName,
        policy =>
        {
            // Allow requests from your Next.js development URL
            policy.WithOrigins(
            "https://localhost:3000",
            "https://app.code-explainer.com:3000",
            "https://app.code-explainer.com"
        )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

// Add services to the container.
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddScoped<AuditInterceptor>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IUserContext, UserContext>();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContextFactory<OrganizationDbContext>(
    (serviceProvider, options) => {
        var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();

        options.UseSqlServer(connectionString)
            .AddInterceptors(auditInterceptor);
    },
    ServiceLifetime.Scoped);

// For Identity, MassTransit, and Scoped Services
builder.Services.AddDbContext<OrganizationDbContext>((serviceProvider, options) =>
{
    var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
    options.UseSqlServer(connectionString).AddInterceptors(auditInterceptor);
}, ServiceLifetime.Scoped);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddIdentity<Staff, IdentityRole<Guid>>(
        options =>
        {
            options.Password.RequiredLength = 6;
        })
    .AddEntityFrameworkStores<OrganizationDbContext>();


builder.Services.AddAuthentication(options =>
{
    // Set ALL defaults to the JWT Bearer scheme
    options.DefaultAuthenticateScheme =
        options.DefaultScheme =
            options.DefaultSignInScheme =
                options.DefaultSignOutScheme =
                    options.DefaultChallengeScheme =
                        options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    builder.Configuration["JWT:SigningKey"])),

        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies[Token.AccessToken];
                return Task.CompletedTask;
            }
        };

    });


builder.Services.AddMassTransit(
    busRegistrationConfigurator =>
    {
        busRegistrationConfigurator.AddEntityFrameworkOutbox<OrganizationDbContext>(outboxConfigurator =>
        {
            outboxConfigurator.QueryDelay = TimeSpan.FromSeconds(5);
            outboxConfigurator.UseSqlServer().UseBusOutbox();
        });

        busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
        busRegistrationConfigurator.UsingRabbitMq((context, cfg) =>
        {
            var rabbitMQHost = builder.Configuration["RabbitMQHost"];
            var rabbitMQPort = ushort.Parse(builder.Configuration["RabbitMQPort"]);
            cfg.Host(
                rabbitMQHost,
                rabbitMQPort,
                "/",
                h =>
                {
                    h.Username(builder.Configuration["RabbitMQUsername"]);
                    h.Password(builder.Configuration["RabbitMQPassword"]);
                });
            cfg.ConfigureEndpoints(context);
        });
    });


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseCors(devPolicyName);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Starting Organization Service");
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<OrganizationDbContext>>();

    await using var dbContext = await dbContextFactory.CreateDbContextAsync();
    Console.WriteLine("--> Attempting to run migrations");
    try
    {
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during migration: {ex.Message}");
    }
}

Console.WriteLine("OrganizationService successfully configured");

app.Run();

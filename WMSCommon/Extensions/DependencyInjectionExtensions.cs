using JasperFx.CodeGeneration;
using JasperFx.Resources;
using MassTransit;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

using Serilog;

using WMSCommon.Constants;
using WMSCommon.DbContexts;
using Wolverine;
using Wolverine.EntityFrameworkCore;
using Wolverine.RabbitMQ;
using Wolverine.SqlServer;
using Wolverine.Transports;

namespace WMSCommon.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static void AddAppSettingsConfig(this WebApplicationBuilder builder)
        {
            var contentRootPath = builder.Environment.ContentRootPath;
            var parentPath = Directory.GetParent(contentRootPath);
            string wmsCommonPath = Path.Combine(parentPath.FullName, Config.CommonFolderName);

            // Load the production configuration first
            builder.Configuration.AddJsonFile(
                Path.Combine(wmsCommonPath, Config.CommonAppSettingsFilename),
                optional: true,
                reloadOnChange: true);

            // Then, load the local appsettings.json.
            // This allows local settings to override shared ones.
            builder.Configuration.AddJsonFile(
                Config.AppSettingsFilename,
                optional: false,
                reloadOnChange: true);

            // load the development configuration to override it
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddJsonFile(
                    Path.Combine(wmsCommonPath, Config.CommonAppSettingsDevFilename),
                    optional: true,
                    reloadOnChange: true);

                builder.Configuration.AddJsonFile(
                    Config.AppSettingsDevFilename,
                    optional: false,
                    reloadOnChange: true);
            }
        }

        public static IServiceCollection AddFrontendCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Config.CorsPolicyName,
                    policy =>
                    {
                        // Allow requests from your Next.js development URL
                        policy.WithOrigins(
                            "https://localhost:3000",
                            $"{Config.SiteUrl}:3000",
                            Config.SiteUrl
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });
            return services;
        }

        public static ConfigureHostBuilder AddSerilog(this ConfigureHostBuilder hostBuilder)
        {
            hostBuilder.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
            return hostBuilder;
        }

        public static IServiceCollection AddFrontendAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
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
                        ValidIssuer = configuration[Config.JwtIssuerKey],
                        ValidateAudience = true,
                        ValidAudience = configuration[Config.JwtAudienceKey],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                configuration[Config.JwtSigningKey])),

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

            return services;
        }

        public static IServiceCollection AddMessageBus<TContext>(
            this IServiceCollection services, 
            IConfiguration configuration,
            Action<WolverineOptions>? configureExtras = null)
            where TContext : DbContext
        {
            string connectionString = configuration.GetConnectionString("Default")!;
            services.AddWolverine(ExtensionDiscovery.ManualOnly, options =>
            {
                options.CodeGeneration.TypeLoadMode = TypeLoadMode.Auto;
                options.PersistMessagesWithSqlServer(connectionString);
                options.UseEntityFrameworkCoreTransactions();
                options.Services.AddDbContextWithWolverineIntegration<TContext>(x => x.UseSqlServer(connectionString));
                options.Services.AddResourceSetupOnStartup();

                options.UseRabbitMq(c =>
                    {
                        c.HostName = configuration[Config.MQHost]!;
                        c.Port = int.Parse(configuration[Config.MQPort]!);
                        c.UserName = configuration[Config.MQUsername]!;
                        c.Password = configuration[Config.MQPassword]!;
                    })
                    .AutoProvision()
                    .UseConventionalRouting(NamingSource.FromHandlerType);
                options.Policies.DisableConventionalLocalRouting();
                
                configureExtras?.Invoke(options);
            });
            
            return services;
        }

        public static IServiceCollection AddAppDbContext<TContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TContext : DbContext
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddDbContext<TContext>((serviceProvider, options) =>
            {
                var auditInterceptor = serviceProvider.GetRequiredService<AuditInterceptor>();
                options.UseSqlServer(connectionString).AddInterceptors(auditInterceptor);
            });

            return services;
        }
    }
}

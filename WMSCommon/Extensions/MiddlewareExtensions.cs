using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using WMSCommon.Constants;

namespace WMSCommon.Extensions
{
    public static class MiddlewareExtensions
    {
        public static WebApplication SetupMiddleware(this WebApplication app)
        {
            // 1. Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseCors(Config.CorsPolicyName);
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }

        public static async Task ApplyMigrations<TDbContext>(
            this WebApplication app, 
            string serviceName)
            where TDbContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<WebApplication>>();

            logger.LogInformation("Starting {ServiceName}", serviceName);

            try
            {
                var dbContext = services.GetRequiredService<TDbContext>();
                logger.LogInformation("--> Attempting to run migrations...");
                await dbContext.Database.MigrateAsync();

                logger.LogInformation("{ServiceName} successfully configured and migrated", serviceName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during migration for {ServiceName}", serviceName);
                // Optionally rethrow if you want the app to fail-fast
                throw;
            }
        }
    }
}

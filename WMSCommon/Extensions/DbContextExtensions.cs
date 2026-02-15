using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using WMSCommon.DbContexts;

namespace WMSCommon.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddWmsDbContext<TContext>(
            this IServiceCollection services,
            string connectionString) where TContext : DbContext
        {
            // 1. Register the Interceptor
            services.AddScoped<AuditInterceptor>();

            // 2. Configure the DbContext
            services.AddDbContext<TContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditInterceptor>();

                options.UseSqlServer(connectionString)
                    .AddInterceptors(interceptor);
            });

            return services;
        }
    }
}

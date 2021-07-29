using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SevenPeaks.VTS.Persistence.ExtensionMethods
{
   public static class EFExtension
    {
        public static IServiceCollection AddSqlPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var conString = configuration.GetSection("ConnectionStrings:DefaultConnection");
            services.AddDbContext<IDatabaseService, DatabaseService>(options =>
            options.UseSqlServer(conString.Value, b => b.MigrationsAssembly("SevenPeaks.VTS.Persistence")));
            return services;
        }
        public static IServiceCollection AddSqlLitePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var conString = configuration.GetSection("ConnectionStrings:DefaultConnection");
            services.AddDbContext<IDatabaseService, DatabaseService>(options =>
            options.UseSqlite(conString.Value, b => b.MigrationsAssembly("SevenPeaks.VTS.Persistence")));
             
            return services;
        }
    }
}

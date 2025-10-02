using Microsoft.EntityFrameworkCore;

namespace IdentityService.Extensions.ServiceCollections;

public static class EfCoreExtensions
{
    public static void RegisterNpgSqlDbContexts<TAppDbContext>(this IServiceCollection services,
        string connectionString)
        where TAppDbContext : DbContext
    {
        services.AddDbContextPool<DbContext, TAppDbContext>((_, opts) =>
        {
            opts.UseNpgsql(connectionString,
                options => { options.MigrationsAssembly(typeof(Program).Assembly.FullName); });
        });
    }
}
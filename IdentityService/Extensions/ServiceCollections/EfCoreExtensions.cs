using IdentityService.DataAccess;
using IdentityService.DataAccess.DataSets.Identity;
using IdentityService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
    
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IEmailSender, EmailSender>();
    }
}
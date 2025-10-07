using IdentityService.DataAccess.DataSets.Identity;
using IdentityService.DataAccess.DataSets.OpenIddict;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DataAccess;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser, ApplicationRole, long>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseOpenIddict<ApplicationEntity, AuthorizationEntity, ScopeEntity, TokenEntity, long>();
    }
}
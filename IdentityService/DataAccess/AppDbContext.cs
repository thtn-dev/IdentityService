using IdentityService.DataAccess.DataSets.OpenIddict;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DataAccess;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.UseOpenIddict<ApplicationEntity, AuthorizationEntity, ScopeEntity, TokenEntity, long>();
    }
}
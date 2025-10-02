using OpenIddict.EntityFrameworkCore.Models;

namespace IdentityService.DataAccess.DataSets.OpenIddict;

public class ScopeEntity : OpenIddictEntityFrameworkCoreScope<long>
{
    public long Id { get; set; }
}
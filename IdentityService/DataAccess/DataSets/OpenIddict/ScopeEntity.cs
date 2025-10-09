using OpenIddict.EntityFrameworkCore.Models;

namespace IdentityService.DataAccess.DataSets.OpenIddict;

public class ScopeEntity : OpenIddictEntityFrameworkCoreScope<long>
{
    public override long Id { get; set; }
}
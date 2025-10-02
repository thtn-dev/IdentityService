using OpenIddict.EntityFrameworkCore.Models;

namespace IdentityService.DataAccess.DataSets.OpenIddict;

public class TokenEntity : OpenIddictEntityFrameworkCoreToken<long, ApplicationEntity, AuthorizationEntity>
{
    public long Id { get; set; }
}
using OpenIddict.EntityFrameworkCore.Models;

namespace IdentityService.DataAccess.DataSets.OpenIddict;

public class AuthorizationEntity : OpenIddictEntityFrameworkCoreAuthorization<long, ApplicationEntity, TokenEntity>
{
    public long Id { get; set; }
}
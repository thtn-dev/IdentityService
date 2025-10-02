using OpenIddict.EntityFrameworkCore.Models;

namespace IdentityService.DataAccess.DataSets.OpenIddict;

public class ApplicationEntity : OpenIddictEntityFrameworkCoreApplication<long, AuthorizationEntity, TokenEntity>
{
    public long Id { get; set; }
}
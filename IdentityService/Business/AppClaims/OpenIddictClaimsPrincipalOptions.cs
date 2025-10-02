using IdentityService.Shared.CustomTypes;

namespace IdentityService.Business.AppClaims;

public class OpenIddictClaimsPrincipalOptions
{
    public ITypeList<IOpenIddictClaimsPrincipalHandler> ClaimsPrincipalHandlers { get; } =
        new TypeList<IOpenIddictClaimsPrincipalHandler>();
}
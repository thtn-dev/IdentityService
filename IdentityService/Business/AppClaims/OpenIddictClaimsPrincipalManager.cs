﻿using System.Security.Claims;
using Microsoft.Extensions.Options;
using OpenIddict.Abstractions;
using OpenIddict.Server;

namespace IdentityService.Business.AppClaims;

public sealed class OpenIddictClaimsPrincipalManager(
    IServiceScopeFactory serviceScopeFactory,
    IOptions<OpenIddictClaimsPrincipalOptions> options)
{
    private IServiceScopeFactory ServiceScopeFactory { get; } = serviceScopeFactory;
    private IOptions<OpenIddictClaimsPrincipalOptions> Options { get; } = options;

    public async Task HandleAsync(OpenIddictRequest openIddictRequest, ClaimsIdentity identity)
    {
        using var scope = ServiceScopeFactory.CreateScope();
        foreach (var providerType in Options.Value.ClaimsPrincipalHandlers)
        {
            var provider = (IOpenIddictClaimsPrincipalHandler)scope.ServiceProvider.GetRequiredService(providerType);
            await provider.HandleAsync(
                new OpenIddictClaimsPrincipalHandlerContext(scope.ServiceProvider, openIddictRequest, identity));
        }
    }
}
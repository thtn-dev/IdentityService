using IdentityService.Business.AppClaims;
using IdentityService.DataAccess;
using IdentityService.DataAccess.DataSets.OpenIddict;

namespace IdentityService.Extensions.ServiceCollections;

public static class OpenIddictExtensions
{
    public static void ConfigureOpenIddict(this IServiceCollection services, IConfiguration configuration)
    {
        var openIddictBuilder = services.AddOpenIddict();
        openIddictBuilder.AddCore(options =>
        {
            options.UseEntityFrameworkCore()
                .UseDbContext<AppDbContext>()
                .ReplaceDefaultEntities<ApplicationEntity, AuthorizationEntity, ScopeEntity, TokenEntity, long>();

            options.UseQuartz();
        });
        //.AddClient(options =>
        //{
        //    options.AllowAuthorizationCodeFlow();

        //    options.AddDevelopmentEncryptionCertificate()
        //        .AddDevelopmentSigningCertificate();

        //    options.UseAspNetCore()
        //        .EnableStatusCodePagesIntegration()
        //        .EnableRedirectionEndpointPassthrough()
        //        .EnableRedirectionEndpointPassthrough();

        //    options.UseSystemNetHttp()
        //        .SetProductInformation(typeof(Program).Assembly);

        //    var externalProviderSettings = configuration.GetSection(nameof(ExternalProviderOptions))
        //        .Get<ExternalProviderOptions>();
        //    if (externalProviderSettings == null)
        //        throw new NullReferenceException($"{nameof(ExternalProviderOptions)} was not configured");

        //    options.UseWebProviders()
        //        .AddGoogle(googleOptions =>
        //        {
        //            var googleSetting = externalProviderSettings.Google ??
        //                                throw new NullReferenceException(
        //                                    $"{nameof(ExternalProviderOptions.Google)} was not configured");
        //            googleOptions.SetClientId(googleSetting.ClientId)
        //                .SetClientSecret(googleSetting.ClientSecret)
        //                .SetRedirectUri(googleSetting.RedirectUri);
        //            googleOptions.AddScopes("email", "profile");
        //        });
        //})

        openIddictBuilder.AddServer(options =>
        {
            options.SetAuthorizationEndpointUris(configuration["OpenIddict:Endpoints:Authorization"]!)
                .SetTokenEndpointUris(configuration["OpenIddict:Endpoints:Token"]!)
                .SetEndSessionEndpointUris(configuration["OpenIddict:Endpoints:Logout"]!)
                .SetIntrospectionEndpointUris(configuration["OpenIddict:Endpoints:Introspection"]!)
                .SetUserInfoEndpointUris(configuration["OpenIddict:Endpoints:Userinfo"]!)
                .SetRevocationEndpointUris(configuration["OpenIddict:Endpoints:Revocation"]!)
                .SetEndUserVerificationEndpointUris(configuration["OpenIddict:Endpoints:EndUserVerification"]!);

            options.AllowClientCredentialsFlow()
                .AllowAuthorizationCodeFlow()
                .AllowImplicitFlow()
                .AllowHybridFlow()
                .AllowRefreshTokenFlow();

            options.UseAspNetCore()
                .EnableAuthorizationEndpointPassthrough()
                .EnableTokenEndpointPassthrough()
                .EnableUserInfoEndpointPassthrough()
                .EnableEndSessionEndpointPassthrough()
                .EnableStatusCodePagesIntegration();

            options.AddDevelopmentEncryptionCertificate()
                .AddDevelopmentSigningCertificate();
            // Note: an ephemeral signing key is deliberately used to make the "OP-Rotation-OP-Sig"
            // test easier to run as restarting the application is enough to rotate the keys.
            options.AddEphemeralEncryptionKey()
                .AddEphemeralSigningKey();

            options.DisableAccessTokenEncryption();
            options.RegisterScopes(configuration.GetSection("OpenIddict:Scopes").Get<string[]>()!);
            options.RegisterClaims(configuration.GetSection("OpenIddict:Claims").Get<string[]>()!);
        });

        openIddictBuilder.AddValidation(options =>
        {
            // Import the configuration from the local OpenIddict server instance.
            options.UseLocalServer();

            // Register the ASP.NET Core host.
            options.UseAspNetCore();

            // Enable authorization entry validation, which is required to be able
            // to reject access tokens retrieved from a revoked authorization code.
            options.EnableAuthorizationEntryValidation();
        });

        services.AddTransient<DefaultOpenIddictClaimsPrincipalHandler>();
        services.Configure<OpenIddictClaimsPrincipalOptions>(options =>
        {
            options.ClaimsPrincipalHandlers.Add<DefaultOpenIddictClaimsPrincipalHandler>();
        });
        services.AddScoped<OpenIddictClaimsPrincipalManager>();
    }
}
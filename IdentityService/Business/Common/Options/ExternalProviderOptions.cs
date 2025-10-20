namespace IdentityService.Business.Common.Options;

public class ExternalProviderOptions
{
    public GoogleOptions? Google { get; set; }
}

public sealed class GoogleOptions
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string RedirectUri { get; set; }
}
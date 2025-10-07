namespace IdentityService.ViewModels;
public class AuthorizeViewModel
{
    public required string? ApplicationName { get; set; }

    public required string? Scope { get; set; }
}
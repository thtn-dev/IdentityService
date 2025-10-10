using IdentityService.BackgroundServices;
using IdentityService.Extensions.ServiceCollections;
using OpenIddict.Validation.AspNetCore;
using IdentityService.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        builder.Services.ConfigureIdentity();
        builder.Services.RegisterNpgSqlDbContexts<DataAccess.AppDbContext>(
            builder.Configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
        builder.Services.ConfigureOpenIddict(builder.Configuration);
        builder.Services.ConfigureQuartz();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/identity/account/login";
            options.Cookie.Name = "dpn_auth";
        });
        builder.Services.AddAntiforgery(options =>
        {
            options.Cookie.Name = "dpn_xsrf";
            options.HeaderName = "X_XSRF_TOKEN";
        });
        builder.Services.AddAuthorization();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("*",
                builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddHostedService<SeedDataWorker>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseHttpsRedirection();
        app.UseCors("*");
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapStaticAssets();
        app.MapControllers();
        app.MapRazorPages();
        app.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.UseAuthorization();
        await app.RunAsync();
    }
}
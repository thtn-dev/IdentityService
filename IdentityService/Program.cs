using IdentityService.Extensions.ServiceCollections;

namespace IdentityService;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.RegisterNpgSqlDbContexts<DataAccess.AppDbContext>(
            builder.Configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
        builder.Services.ConfigureOpenIddict(builder.Configuration);
        builder.Services.ConfigureQuartz();
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

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment()) app.MapOpenApi();

        app.UseHttpsRedirection();

        app.UseAuthorization();
        await app.RunAsync();
    }
}
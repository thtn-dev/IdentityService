using Quartz;

namespace IdentityService.Extensions.ServiceCollections;

public static class QuartzExtensions
{
    public static void ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            options.UseSimpleTypeLoader();
            options.UseInMemoryStore();
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
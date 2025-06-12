using Microsoft.Extensions.DependencyInjection;
using NoteAPI.Services.Jobs;
using Quartz;

namespace NoteAPI.API.Jobs
{
    public static class QuartzExtensions
    {
        public static void AddQuartzJobs(this IServiceCollection services)
        {
            services.AddQuartz(q =>
            {
                var jobKey = new JobKey("KlineFetchJob");

                q.AddJob<KlineFetchJob>(opts => opts.WithIdentity(jobKey));

                q.AddTrigger(opts => opts
                    .ForJob(jobKey)
                    .WithIdentity("KlineFetchTrigger")
                    .WithCronSchedule("0 0/15 * * * ?")); // Her 15 dakikada bir
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
    }
}

using System.Reflection;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NoteAPI.API.Common.Settings;
using NoteAPI.Repo.SqlDatabase.Context;
using NoteAPI.Services;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Services;

namespace NoteAPI.IoC.Configuration.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services != null)
            {
                services.AddTransient<INoteService, NoteService>();
            }
        }

        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationInsights = configuration.GetSection(nameof(ApplicationInsights)).Get<ApplicationInsights>();


            services.AddDbContext<NoteAPISqlDbContext>(options => options.UseSqlServer(applicationInsights.ConnectionString));
        }

        public static void ConfigureMappings(this IServiceCollection services)
        {
            if (services != null)
            {
                //Automap settings
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
            }
        }

        public static void ConfigureValidators(this IServiceCollection services)
        {
            if (services != null)
            {
                //services.AddScoped<IValidator<UserCreation>, UserCreationValidation>();
                //services.AddScoped<IValidator<User>, UserValidator>();
            }
        }
    }
}

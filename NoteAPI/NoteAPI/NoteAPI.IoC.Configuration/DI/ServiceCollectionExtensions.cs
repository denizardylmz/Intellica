using System.Reflection;

using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NoteAPI.Services;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Model;
using NoteAPI.Services.Validators;

namespace NoteAPI.IoC.Configuration.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services != null)
            {
                services.AddTransient<IUserService, UserService>();
            }
        }

        public static void ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            //TO BE COMPLETED IF NEEDED
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
                services.AddScoped<IValidator<UserCreation>, UserCreationValidation>();
                services.AddScoped<IValidator<User>, UserValidator>();
            }
        }
    }
}

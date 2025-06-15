using System;
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
using NoteAPI.Services.Validators;
using Telegram.Bot;

namespace NoteAPI.IoC.Configuration.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            if (services != null)
            {
                var externalServices = configuration.GetSection("ExternalServices").Get<ExternalServices>();
                
                services.AddHttpClient(externalServices.ForecastService.Name, c => c.BaseAddress = new Uri(externalServices.ForecastService.BaseUrl));
                services.AddHttpClient(externalServices.OllamaModel.Name, c => c.BaseAddress = new Uri(externalServices.OllamaModel.BaseUrl));

                services.AddScoped<IExternalApiService, ExternalApiService>();
                services.AddScoped<IOllamaChatBotService, OllamaChatBotService>();
                services.AddScoped<IBinanceService, BinanceService>();
                services.AddScoped<INoteService, NoteService>();
                services.AddScoped<IUserService, UserService>();

                services.AddSingleton<ITelegramBotClient>(sp =>
                {
                    var config = sp.GetRequiredService<IConfiguration>();
                    return new TelegramBotClient(config["Telegram:BotToken"]); //Telegram Botu eklenecek!
                });


                services.AddHttpContextAccessor();
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
                services.AddValidatorsFromAssemblyContaining<NoteValidation>();
            }
        }
    }
}

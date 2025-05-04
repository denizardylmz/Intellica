using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NoteAPI.API.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        const string DB_CONNECTION_STRING = "NoteAPIConnectionString";

        public static IServiceCollection AddDBHealthChecks<T>(this IServiceCollection services, IConfiguration configuration)
            where T : DbContext
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            string connectionString = configuration.GetConnectionString(DB_CONNECTION_STRING);

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            //services.AddHealthChecks().AddSqlServer(connectionString);
            services.AddHealthChecks().AddDbContextCheck<T>();

            return services;
        }
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            IConfigurationSection servicesSettings = configuration.GetSection("ServicesSettings");

            if (servicesSettings != null)
            {
                string tierServiceName = servicesSettings.GetValue<string>("TierService:Name");
                //string securityServiceName = servicesSettings.GetValue<string>("SecurityService:Name");

                if (string.IsNullOrEmpty(tierServiceName))
                    services.AddHealthChecks();
                else
                    services.AddHealthChecks()
                        .AddCheck<TierServiceHealthCheck>(tierServiceName);
                //.AddCheck<SecurityServiceHealthCheck>(securityServiceName);
            }

            return services;
        }
    }
}

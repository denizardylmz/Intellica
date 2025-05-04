using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace NoteAPI.API.Common.Extensions
{
    public class TierServiceHealthCheck : IHealthCheck
    {
        readonly IConfiguration _configuration;
        readonly IServiceProvider _serviceProvider;
        readonly ILogger<TierServiceHealthCheck> _logger;

        string serviceName = string.Empty;

        public TierServiceHealthCheck(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            ILogger<TierServiceHealthCheck> logger)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _logger = logger;

            IConfigurationSection servicesSettings = _configuration.GetSection("ServicesSettings");

            serviceName = servicesSettings.GetValue<string>("TierService:Name");
        }


        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {            
            try
            {
                var client = _serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(serviceName);
                var response = await client.GetAsync("/health");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Service is healthy"); //TODO: review this if needed
                    return HealthCheckResult.Healthy($"{serviceName} is healthy");
                }
                else
                {
                    _logger.LogWarning(
                        new Exception(
                            $"{response.StatusCode}{Environment.NewLine}{response.ReasonPhrase}"
                            ),
                        $"{serviceName} is unhealthy"
                        );
                    return HealthCheckResult.Unhealthy($"{serviceName} is unhealthy");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{serviceName} is unhealthy::ERROR::{Environment.NewLine}{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return HealthCheckResult.Unhealthy($"{serviceName} is unhealthy");
            }
        }
    }
}

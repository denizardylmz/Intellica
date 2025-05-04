using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NoteAPI.API.Common.Settings;
using NoteAPI.IoC.Configuration.DI;
using NoteAPI.Tools.Configurations;

namespace NoteAPI.Services.Tests
{
    [TestClass]
    public class TestBase
    {
        internal IConfigurationRoot _configurationRoot;
        internal ServiceCollection _services;
        internal ServiceProvider _serviceProvider;

        public TestBase()
        {
            _configurationRoot = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory());
            var appSettings = _configurationRoot.GetSection(nameof(AppSettings));

            _services = new ServiceCollection();

            _services.AddLogging();
            _services.Configure<AppSettings>(appSettings);

            //Mappings
            _services.ConfigureMappings();

            //Business settings
            _services.ConfigureBusinessServices(_configurationRoot);

            //Repositories
            _services.ConfigureRepositories(_configurationRoot);

            //Business validators
            _services.ConfigureValidators();

            //We load EXACTLY the same settings (DI and others) than API real solution, what is much better for tests.
            _services.ConfigureBusinessServices(_configurationRoot);

            _serviceProvider = _services.BuildServiceProvider();
        }

        ~TestBase()
        {
            if (_serviceProvider != null)
                _serviceProvider.Dispose();
        }
    }
}

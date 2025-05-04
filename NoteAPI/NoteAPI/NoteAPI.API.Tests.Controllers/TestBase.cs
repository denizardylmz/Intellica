using System;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NoteAPI.API.Common.Settings;
using NoteAPI.IoC.Configuration.DI;
using NoteAPI.Tools.Configurations;

namespace NoteAPI.API.Tests.Controllers
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

            _services.ConfigureMappings();

            //Business settings
            _services.ConfigureBusinessServices((IConfiguration)_configurationRoot);

            //Repositories
            _services.ConfigureRepositories((IConfiguration)_configurationRoot);

            //Business validators
            _services.ConfigureValidators();

            _serviceProvider = _services.BuildServiceProvider();
        }

        ~TestBase()
        {
            if (_serviceProvider != null)
                _serviceProvider.Dispose();
        }
    }
}

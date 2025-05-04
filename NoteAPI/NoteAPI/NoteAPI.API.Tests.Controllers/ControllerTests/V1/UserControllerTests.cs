using System;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NoteAPI.API.Controllers.V2;
using NoteAPI.API.DataContracts;
using NoteAPI.API.DataContracts.Requests;
using NoteAPI.Services.Contracts;

namespace NoteAPI.API.Tests.Controllers.ControllerTests.V1
{
    [TestClass]
    public class UserControllerTests : TestBase
    {
        //NOTE: should be replaced by an interface
        UserControllerV2 _controller;

        public UserControllerTests() : base()
        {
            var businessService = _serviceProvider.GetRequiredService<IUserService>();
            var mapper = _serviceProvider.GetRequiredService<IMapper>();
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<UserControllerV2>();

            _controller = new UserControllerV2(businessService, mapper, logger);
        }

        [TestMethod]
        public async Task CreateUser_Nominal_OK()
        {
            //Simple test
            var user = await _controller.CreateUser(new UserCreationRequest
            {
                User = new User { Id = "U1", Firstname = "Firstname 1", Lastname = "Lastname 1" },
                Date = DateTime.Now
            });

            Assert.IsNotNull(user);
        }


    }
}

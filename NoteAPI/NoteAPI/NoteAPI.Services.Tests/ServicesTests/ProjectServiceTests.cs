using Microsoft.Extensions.DependencyInjection;

using NoteAPI.Services.Contracts;

namespace NoteAPI.Services.Tests
{
    [TestClass]
    public class ProjectServiceTests : TestBase
    {
        IUserService _service;

        public ProjectServiceTests() : base()
        {
            _service = _serviceProvider.GetRequiredService<IUserService>();
        }

        [TestMethod]
        public async Task CreateProject_Nominal_OK()
        {
            //Simple test
            var upsertResult = await _service.CreateAsync(
                 new Services.Model.User
                 {
                     Id = Guid.NewGuid().ToString(),
                     Firstname = "Firstname",
                     Lastname = "Lastname"
                 });

            Assert.IsNotNull(upsertResult);
        }


    }
}

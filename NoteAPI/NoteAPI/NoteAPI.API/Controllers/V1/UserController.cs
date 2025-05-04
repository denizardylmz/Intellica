using System;
using System.Threading.Tasks;

using Asp.Versioning;

using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using NoteAPI.API.DataContracts;
using NoteAPI.API.DataContracts.Requests;
using NoteAPI.Services.Contracts;

using S = NoteAPI.Services.Model;

namespace NoteAPI.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/users")]//required for default versioning
    [Route("api/v{version:apiVersion}/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

#pragma warning disable CS1591
        public UserController(IUserService service, IMapper mapper, ILogger<UserController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }
#pragma warning restore CS1591

        #region GET
        /// <summary>
        /// Returns a user entity according to the provided Id.
        /// </summary>
        /// <remarks>
        /// XML comments included in controllers will be extracted and injected in Swagger/OpenAPI file.
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>
        /// Returns a user entity according to the provided Id.
        /// </returns>
        /// <response code="200">Returns the item with the provided Id.</response>
        /// <response code="204">If the item is null.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(User))]
        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            _logger.LogDebug($"UserControllers::Get::{id}");

            var data = await _service.GetAsync(id);

            if (data != null)
                return _mapper.Map<User>(data);
            else
                return null;
        }
        #endregion

        #region POST
        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="value"></param>
        /// <returns>A newly created user.</returns>
        /// <response code="201">Returns the newly created item.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        public async Task<User> CreateUser([FromBody] UserCreationRequest value)
        {
            _logger.LogDebug($"UserControllers::Post::");

            if (value == null)
                throw new ArgumentNullException("value");

            if (value.User == null)
                throw new ArgumentNullException("value.User");


            var data = await _service.CreateAsync(_mapper.Map<S.User>(value.User));

            if (data != null)
                return _mapper.Map<User>(data);
            else
                return null;

        }
        #endregion

        #region PUT
        /// <summary>
        /// Updates an user entity.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns>
        /// Returns a boolean notifying if the user has been updated properly.
        /// </returns>
        /// <response code="200">Returns a boolean notifying if the user has been updated properly.</response>
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<bool> UpdateUser(User parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return await _service.UpdateAsync(_mapper.Map<S.User>(parameter));
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Deletes an user entity.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="id">User Id</param>
        /// <returns>
        /// Boolean notifying if the user has been deleted properly.
        /// </returns>
        /// <response code="200">Boolean notifying if the user has been deleted properly.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<bool> DeleteDevice(string id)
        {
            return await _service.DeleteAsync(id);
        }
        #endregion

        #region Exceptions
        /// <summary>
        /// Endpoint to raise an exception and test how the exceptions are handled.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("exception/{message}")]
        [ProducesErrorResponseType(typeof(Exception))]
        public async Task RaiseException(string message)
        {
            _logger.LogDebug($"UserControllers::RaiseException::{message}");

            throw new Exception(message);
        }
        #endregion
    }
}

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

namespace NoteAPI.API.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/users")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class UserControllerV2 : Controller
    {
        private readonly IUserService _service;
        private readonly IMapper _mapper;
        private readonly ILogger<UserControllerV2> _logger;

#pragma warning disable CS1591
        public UserControllerV2(IUserService service, IMapper mapper, ILogger<UserControllerV2> logger)
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
        /// 
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
        public async Task<ActionResult<User>> Get(string id)
        {
            _logger.LogDebug($"UserControllers::Get::{id}");

            var data = await _service.GetAsync(id);

            if (data != null)
                return Ok(_mapper.Map<User>(data));
            else
                return NoContent();
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
        public async Task<ActionResult<User>> CreateUser([FromBody] UserCreationRequest value)
        {
            _logger.LogDebug($"UserControllers::Post::");

            if (value == null)
                throw new ArgumentNullException("value");

            if (value.User == null)
                throw new ArgumentNullException("value.User");


            var data = await _service.CreateAsync(_mapper.Map<S.User>(value.User));

            if (data != null)
                return CreatedAtAction("CreateUser", _mapper.Map<User>(data));
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during the creation process.");

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
        public async Task<ActionResult<bool>> UpdateUser(User parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException("parameter");

            return Ok(await _service.UpdateAsync(_mapper.Map<S.User>(parameter)));
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
        public async Task<ActionResult<bool>> DeleteDevice(string id)
        {
            return Ok(await _service.DeleteAsync(id));
        }
        #endregion
    }
}

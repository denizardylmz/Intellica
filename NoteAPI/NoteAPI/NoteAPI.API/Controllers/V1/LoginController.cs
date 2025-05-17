using System.IdentityModel.Tokens.Jwt;
using System;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using NoteAPI.Services.Contracts;
using System.Threading.Tasks;
using NoteAPI.Services.Models;

namespace NoteAPI.API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/login")]//required for default versioning
    [Route("api/v{version:apiVersion}/login")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService   _userService;


        public LoginController(ILogger<LoginController> logger, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
        }


        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> Loging([FromBody] LoginRequest loginRequest)
        {
            var token = await _userService.Login(loginRequest.Email, loginRequest.Password);
            
            if (token == null) return Unauthorized();
            
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            var result = await _userService.CreateUser(user);

            if (user == null) return BadRequest();
            return Ok(result);
        }



    }
}

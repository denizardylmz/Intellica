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



        public LoginController(ILogger<LoginController> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost]
        public IActionResult Loging([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.Email == "admin" && loginRequest.Password == "1234")
            {
                var claims = new[]
                {
            new Claim(ClaimTypes.Name, loginRequest.Email),
            new Claim(ClaimTypes.Role, "Admin"),
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HooppaAPIHooppaAPI1234HooppaAPIHooppaAPI1234"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "Deniz'sNoteAPI",
                    audience: "NoteClient",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            return Unauthorized();
        }

    }
}

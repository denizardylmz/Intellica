using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NoteAPI.API.Common.Settings;
using NoteAPI.API.DataContracts.ExternalAPIContracts;
using NoteAPI.API.DataContracts.Requests;
using NoteAPI.API.DataContracts.Responses;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Services;

namespace NoteAPI.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/forecast")]
[Route("api/v{version:apiVersion}/forecast")]
[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
public class ForecastController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    
    public ForecastController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        var externalServices = configuration.GetSection("ExternalServices").Get<ExternalServices>();
        _httpClient = httpClientFactory.CreateClient("WeatherApi");
    }
    
    
    [AllowAnonymous]
    [HttpPost("current")]
    public async Task<ActionResult<Response<string, WeatherResponse>>> ForecastLast1Week([FromBody] Request<string> request)
    {
        var service = new ExternalApiService(_httpClient);
        var externalServices = _configuration.GetSection("ExternalServices").Get<ExternalServices>();
            
        var queryParams = new List<(string, string)>()
        {
            ("q", request.Payload) 
        }; 
        
        var response = new Response<string, WeatherResponse>(
            request.Payload, 
            service.GetAsync<WeatherResponse>("v1/current.json", queryParams: queryParams , keyName:"key", externalServices.ForecastService.Key)
        );
        await response.ExecuteTask();
        
        if (response.ResponseContent == null) return NoContent();
        return Ok(response);
    }
}
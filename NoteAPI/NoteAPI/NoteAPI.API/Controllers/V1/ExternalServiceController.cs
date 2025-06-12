using System.Collections.Generic;
using System.Configuration;
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
using NoteAPI.Services.Models.OllamaModels;
using NoteAPI.Services.Services;

namespace NoteAPI.API.Controllers.V1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/external")]
[Consumes("application/json")]
[Produces("application/json")]
[ApiController]
public class ForecastController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IExternalApiService _externalApiService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ExternalServices _externalServices;
    private readonly IOllamaChatBotService _ollamaService;
    private readonly Promts _promts;

    public ForecastController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IExternalApiService externalApiService, IOllamaChatBotService ollamaChatBotService)
    {
        _configuration = configuration;
        _externalApiService = externalApiService;
        _httpClientFactory = httpClientFactory;
        _externalServices = _configuration.GetSection("ExternalServices").Get<ExternalServices>();
        _promts = _configuration.GetSection("Promts").Get<Promts>();
        _ollamaService = ollamaChatBotService;

    }


    [AllowAnonymous]
    [HttpPost("forecast/current")]
    public async Task<ActionResult<Response<string, WeatherResponse>>> ForacastCurrent([FromBody] Request<string> request)
    {
        var queryParams = new List<(string, string)>()
        {
            ("q", request.Payload)
        };
        var client = _httpClientFactory.CreateClient(_externalServices.ForecastService.Name);
        _externalApiService.SetClient(client);

        var response = new Response<string, WeatherResponse>(
            request.Payload,
            _externalApiService.GetAsync<WeatherResponse>("v1/current.json", queryParams: queryParams, keyName: "key", _externalServices.ForecastService.Key)
        );

        await response.ExecuteTask();

        if (response.ResponseContent == null) return NoContent();
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("forecast/ai/report")]
    public async Task<ActionResult<Response<string, WeatherResponse>>> Forecast1Week([FromBody] Request<string> request)
    {
        var queryParams = new List<(string, string)>()
        {
            ("days", "3"),
            ("q", request.Payload)
        };
        var client = _httpClientFactory.CreateClient(_externalServices.ForecastService.Name);
        _externalApiService.SetClient(client);

        var response = new Response<string, WeatherResponse>(
            request.Payload,
            _externalApiService.GetAsync<WeatherResponse>("v1/forecast.json", queryParams: queryParams, keyName: "key", _externalServices.ForecastService.Key)
        );

        await response.ExecuteTask();

        var OllamaRequest = new OllamaRequest(_externalServices.OllamaModel.ModelName, request.Payload, system: _promts.WeatherSpecialist);

        var responseOllama = new Response<string, OllamaFullResponse>(
            request.Payload,
            _ollamaService.TalkWithAIAsync(OllamaRequest)
        );
        await responseOllama.ExecuteTask();

        if (responseOllama.ResponseContent == null) return NoContent();
        return Ok(responseOllama);
    }


    [AllowAnonymous]
    [HttpPost("ask")]
    public async Task<ActionResult<Response<OllamaRequest, OllamaFullResponse>>> AskToAI([FromBody] Request<string> request)
    {
        var OllamaRequest = new OllamaRequest(_externalServices.OllamaModel.ModelName, request.Payload);

        var response = new Response<string, OllamaFullResponse>(
            request.Payload,
            _ollamaService.TalkWithAIAsync(OllamaRequest) 
        );
        await response.ExecuteTask();

        if (response.ResponseContent == null) return NoContent();
        return Ok(response);
    }
}
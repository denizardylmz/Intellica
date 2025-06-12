using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteAPI.API.Common.Settings;
using NoteAPI.API.DataContracts.Requests;
using NoteAPI.API.DataContracts.Responses;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Models.OllamaModels;
using Binance.Net.Objects.Models.Spot;
using Binance.Net.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System;
using NoteAPI.Services.Core;
using Microsoft.Extensions.Logging;

namespace NoteAPI.API.Controllers.V1
{
    [Route("api/[controller]/binance")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class BinanceController : ControllerBase
    {
        private readonly IBinanceService _binanceService;
        
        public BinanceController(IBinanceService binanceService, ILogger<BinanceController> logger)
        {
            _binanceService = binanceService;
        }

        [AllowAnonymous]
        [HttpGet("price/current/{symbol}")]
        public async Task<ActionResult<Response<string, BinancePrice>>> AskToAI(string symbol)
        {
            var response = new Response<string, BinancePrice>(
                symbol,
                _binanceService.GetCurrentPriceAsync(symbol)
            );
            await response.ExecuteTask();
            
            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == null) return NoContent();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("rsi/current/{symbol}")]
        public async Task<ActionResult<Response<string, decimal?>>> AskToAI(string symbol, [FromQuery] int day = 14, [ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.OneMinute)
        {
            var response = new Response<string, decimal?>(
                symbol,
                _binanceService.GetRsiAsync(symbol, interval:interval, limit:day)
            );
            await response.ExecuteTask();

            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == null) return NoContent();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task Test()
        {
            await _binanceService.FetchKlineDataToDBAsync();
        }
    }

}

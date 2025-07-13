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
using NoteAPI.Services.Models.Binance;

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
        [HttpGet("rsi/{symbol}")]
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
        [HttpGet("emo/{symbol}")]
        public async Task<ActionResult<Response<string, (List<decimal?>, List<decimal?>)>>> AskToAI(string symbol, [ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var response = new Response<string, (List<decimal?>, List<decimal?>)>(
                symbol,
                _binanceService.GetEMAsync(symbol, interval: interval)
            );
            await response.ExecuteTask();

            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == (null,null)) return NoContent();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("macd/{symbol}")]
        public async Task<ActionResult<Response<string, MacdResult>>> GetMACD(string symbol, [ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var response = new Response<string, MacdResult>(
                symbol,
                _binanceService.GetMACDAsync(symbol, interval)
            );
            await response.ExecuteTask();

            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == null) return NoContent();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("bollinger/{symbol}")]
        public async Task<ActionResult<Response<string, BollingerResult>>> GetBollinger(string symbol, [ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var response = new Response<string, BollingerResult>(
                symbol,
                _binanceService.GetBollingerAsync(symbol, interval)
            );

            await response.ExecuteTask();

            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == null) return NoContent();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("stochrsi/{symbol}")]
        public async Task<ActionResult<Response<string, List<decimal?>>>> GetStochasticRsi(string symbol, [ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var response = new Response<string, List<decimal?>>(
                symbol,
                _binanceService.GetStochasticRsiAsync(symbol, interval)
            );

            await response.ExecuteTask();

            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == null) return NoContent();

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("analysis/{symbol}")]
        public async Task<ActionResult<Response<string, AnalysisResultDto>>> GetAnalysis(string symbol, [ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var response = new Response<string, AnalysisResultDto>(
                symbol,
                _binanceService.GetAnalysisAsync(symbol, interval)
            );
            await response.ExecuteTask();

            if (!response.IsSuccessfull) return BadRequest(response.Error);
            if (response.ResponseContent == null) return NoContent();
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("analysis/ai/{symbol}")]
        public async Task<ActionResult<Response<string, OllamaFullResponse>>> GetAnalysisAI(string symbol,[ModelBinder(BinderType = typeof(KlineIntervalModelBinder))] KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var response = new Response<string, OllamaFullResponse>(
                symbol,
                _binanceService.AnalyzeMarketWithAIAsync(symbol)
            );

            await response.ExecuteTask();

            if (!response.IsSuccessfull)
                return BadRequest(response.Error);
            if (response.ResponseContent == null)
                return NoContent();
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet("PnL/{symbol}")]
        public async Task<IActionResult> Test(string symbol, DateTime startFrom)
        {
            var reuslt = await _binanceService.GetTotalRealizedPnL(symbol, startFrom);
            return Ok(reuslt);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            await _binanceService.GetPnLAllSpots(DateTime.Now.Date.AddDays(-14), true);
            return Ok();
        }
    }

}

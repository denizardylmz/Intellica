using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoteAPI.API.Common.Settings;
using NoteAPI.Repo.SqlDatabase.Context;
using NoteAPI.Repo.SqlDatabase.DTO;
using NoteAPI.Services.Contracts;
using NoteAPI.Services.Core;
using NoteAPI.Services.Models.Binance;
using NoteAPI.Services.Models.OllamaModels;

namespace NoteAPI.Services.Services
{
    public class BinanceService : IBinanceService
    {
        private readonly BinanceSettings _binanceSettings;
        private readonly NoteAPISqlDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BinanceService> _logger;
        private readonly Promts _promts;
        private readonly IOllamaChatBotService _chatBotService;
        private readonly ExternalServices _externalServices;


        public BinanceService(IConfiguration configuration, NoteAPISqlDbContext context, IMapper mapper, ILogger<BinanceService> logger, IOllamaChatBotService chatBotService)
        {
            _context = context;
            _binanceSettings = configuration.GetSection("BinanceSettings").Get<BinanceSettings>();
            _mapper = mapper;
            _logger = logger;
            _promts = configuration.GetSection("Promts").Get<Promts>();
            _externalServices = configuration.GetSection("ExternalServices").Get<ExternalServices>();
            _chatBotService = chatBotService;
        }

        public async Task<decimal?> GetRsiAsync(string symbol, KlineInterval interval = KlineInterval.OneMinute, int limit = 30)
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
            });

            var klinesResult = await client.SpotApi.ExchangeData.GetKlinesAsync(
                symbol: symbol,
                interval: interval,  
                limit: limit + 1); 

            if (!klinesResult.Success)
                return null;

            var closes = klinesResult.Data.Select(k => k.ClosePrice).ToList();
            return StockMathCore.CalculateRSI(closes, limit);
        }

        public async Task<(List<decimal?>, List<decimal?>)> GetEMAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
            });

            var klinesResult = await client.SpotApi.ExchangeData.GetKlinesAsync(
                symbol: symbol,
                interval: interval,
                limit: 150);

            if (!klinesResult.Success)
                return (null,null);

            var closes = klinesResult.Data.Select(k => k.ClosePrice).ToList();
            return StockMathCore.CalculateEMA50and100(closes);
        }

        public async Task<MacdResult> GetMACDAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
            });

            var klinesResult = await client.SpotApi.ExchangeData.GetKlinesAsync(
                symbol: symbol,
                interval: interval,
                limit: 150);

            if (!klinesResult.Success)
                return null;

            var closes = klinesResult.Data.Select(k => k.ClosePrice).ToList();

            return StockMathCore.CalculateMACD(closes);
        }

        public async Task<BollingerResult> GetBollingerAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
            });

            var klinesResult = await client.SpotApi.ExchangeData.GetKlinesAsync(
                symbol: symbol,
                interval: interval,
                limit: 100);

            if (!klinesResult.Success)
                return null;

            var closes = klinesResult.Data.Select(k => k.ClosePrice).ToList();
            return StockMathCore.CalculateBollingerBands(closes);
        }

        public async Task<List<decimal?>> GetStochasticRsiAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
            });

            var klinesResult = await client.SpotApi.ExchangeData.GetKlinesAsync(
                symbol: symbol,
                interval: interval,
                limit: 100
            );

            if (!klinesResult.Success)
                return null;

            var closes = klinesResult.Data.Select(k => k.ClosePrice).ToList();
            return StockMathCore.CalculateStochasticRsi(closes);
        }

        public async Task<AnalysisResultDto> GetAnalysisAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            var rsi = await GetRsiAsync(symbol, interval, 14);
            var (ema50, ema100) = await GetEMAsync(symbol, interval);
            var macd = await GetMACDAsync(symbol, interval);
            var bollinger = await GetBollingerAsync(symbol, interval);
            var stochasticRsi = await GetStochasticRsiAsync(symbol, interval);

            return new AnalysisResultDto
            {
                Rsi = rsi,
                Ema50 = ema50,
                Ema100 = ema100,
                MacdLine = macd?.MacdLine,
                SignalLine = macd?.SignalLine,
                Histogram = macd?.Histogram,
                UpperBand = bollinger?.UpperBand,
                MiddleBand = bollinger?.MiddleBand,
                LowerBand = bollinger?.LowerBand,
                StochasticRsi = stochasticRsi
            };
        }


        public async Task FetchKlineDataToDBAsync()
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
            });

            KlineInterval interval;
            var IntervalMapper = new KlineIntervalModelBinder();
            if (!IntervalMapper.TryParse(_binanceSettings.BinanceKlineFetchOptions.Interval, out interval))
            {
                throw new Exception($"Invalid interval: {_binanceSettings.BinanceKlineFetchOptions.Interval}");
            }

            foreach (var _symbol in _binanceSettings.BinanceKlineFetchOptions.Symbols)
            {
                var klinesResult = await client.SpotApi.ExchangeData.GetKlinesAsync(
                symbol: _symbol,
                interval: interval,
                limit: _binanceSettings.BinanceKlineFetchOptions.Limit);

                if (!klinesResult.Success)
                {
                    _logger.LogWarning($"Failed to fetch data for {_symbol}: {klinesResult.Error}");
                    continue;
                }

                var existingSet = _context.BinanceKlines.Where(x => x.Symbol == _symbol)
                                                        .Select(x => x.OpenTime)
                                                        .ToHashSet();

                var mappedList = klinesResult.Data
                                    .Where(k => !existingSet.Contains(k.OpenTime))
                                    .Select(k =>
                                    {
                                        var entity = _mapper.Map<BinanceKline>(k);
                                        entity.Symbol = _symbol;
                                        return entity;
                                    })
                                    .ToList();

                if (mappedList.Any())
                {
                    try
                    {
                        await _context.BinanceKlines.AddRangeAsync(mappedList);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning($"Failed to Add data to Db for {_symbol}: {e.Message} - {e.InnerException?.Message}");
                        continue;
                    }                    
                }
            }
        }


        public async Task<OllamaFullResponse> AnalyzeMarketWithAIAsync(string symbol, KlineInterval interval = KlineInterval.FifteenMinutes)
        {
            try
            {
                var indicators = await GetAnalysisAsync(symbol, interval);
                var prompt = GenerateAnalysisPrompt(symbol, indicators);

                var ollamaRequest = new OllamaRequest(
                    _externalServices.OllamaModel.ModelName,
                    prompt,
                    system: _promts.CryptoAnalyst
                );

                var result = await _chatBotService.TalkWithAIAsync(ollamaRequest);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"AI analysis failed for symbol: {symbol}");
                return null;
            }
        }

        private string GenerateAnalysisPrompt(string symbol, AnalysisResultDto indicators)
        {
            
            var interpolated = _promts.CryptoAnalyst
                .Replace("{{symbol}}", symbol)
                .Replace("{{rsi}}", indicators.Rsi?.ToString() ?? "N/A")
                .Replace("{{ema50}}", string.Join(", ", indicators.Ema50?.TakeLast(5) ?? Enumerable.Empty<decimal?>()))
                .Replace("{{ema100}}", string.Join(", ", indicators.Ema100?.TakeLast(5) ?? Enumerable.Empty<decimal?>()))
                .Replace("{{macdLine}}", string.Join(", ", indicators.MacdLine?.TakeLast(5) ?? Enumerable.Empty<decimal?>()))
                .Replace("{{signalLine}}", string.Join(", ", indicators.SignalLine?.TakeLast(5) ?? Enumerable.Empty<decimal?>()))
                .Replace("{{histogram}}", string.Join(", ", indicators.Histogram?.TakeLast(5) ?? Enumerable.Empty<decimal?>()))
                .Replace("{{upperBand}}", indicators.UpperBand?.LastOrDefault()?.ToString() ?? "N/A")
                .Replace("{{middleBand}}", indicators.MiddleBand?.LastOrDefault()?.ToString() ?? "N/A")
                .Replace("{{lowerBand}}", indicators.LowerBand?.LastOrDefault()?.ToString() ?? "N/A")
                .Replace("{{stochRsi}}", string.Join(", ", indicators.StochasticRsi?.TakeLast(5) ?? Enumerable.Empty<decimal?>()));

            return interpolated;
        }


        public async Task<BinancePrice> GetCurrentPriceAsync(string symbol)
        {
            var client = new BinanceRestClient(options =>
            {
                options.ApiCredentials = new ApiCredentials(_binanceSettings.Key, _binanceSettings.Secret);
                options.AutoTimestamp = true;
                options.OutputOriginalData = false;
            });

            var result = await client.SpotApi.ExchangeData.GetPriceAsync(symbol);

            return result.Data;
        }
    }
}

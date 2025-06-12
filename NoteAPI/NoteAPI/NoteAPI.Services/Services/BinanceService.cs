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

namespace NoteAPI.Services.Services
{
    public class BinanceService : IBinanceService
    {
        private readonly BinanceSettings _binanceSettings;
        private readonly NoteAPISqlDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<BinanceService> _logger;

        public BinanceService(IConfiguration configuration, NoteAPISqlDbContext context, IMapper mapper, ILogger<BinanceService> logger)
        {
            _context = context;
            _binanceSettings = configuration.GetSection("BinanceSettings").Get<BinanceSettings>();
            _mapper = mapper;
            _logger = logger;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NoteAPI.Services.Core
{
    public class KlineIntervalModelBinder : IModelBinder
    {
        private static readonly Dictionary<string, KlineInterval> _map = new Dictionary<string, KlineInterval>(StringComparer.OrdinalIgnoreCase)
                                                                            {
                                                                                { "1s", KlineInterval.OneSecond },
                                                                                { "1m", KlineInterval.OneMinute },
                                                                                { "3m", KlineInterval.ThreeMinutes },
                                                                                { "5m", KlineInterval.FiveMinutes },
                                                                                { "15m", KlineInterval.FifteenMinutes },
                                                                                { "30m", KlineInterval.ThirtyMinutes },
                                                                                { "1h", KlineInterval.OneHour },
                                                                                { "2h", KlineInterval.TwoHour },
                                                                                { "4h", KlineInterval.FourHour },
                                                                                { "6h", KlineInterval.SixHour },
                                                                                { "8h", KlineInterval.EightHour },
                                                                                { "12h", KlineInterval.TwelveHour },
                                                                                { "1d", KlineInterval.OneDay },
                                                                                { "3d", KlineInterval.ThreeDay },
                                                                                { "1w", KlineInterval.OneWeek }
                                                                            };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue;

                if (string.IsNullOrEmpty(value))
                {
                    bindingContext.Result = ModelBindingResult.Failed();
                    return Task.CompletedTask;
                }

                if (_map.TryGetValue(value, out var result))
                {
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"Invalid interval '{value}'");
                    bindingContext.Result = ModelBindingResult.Failed();
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in KlineIntervalModelBinder", ex);
            }
        }

        public bool TryParse(string input, out KlineInterval interval) => _map.TryGetValue(input, out interval);
    }
}

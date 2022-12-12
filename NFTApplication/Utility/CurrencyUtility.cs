// <copyright company="MyCOM Global LTD" author="Chris McGorty">
//     Copyright (c) 2022 All Rights Reserved
// </copyright>
//
using System.Globalization;
using Microsoft.Extensions.Caching.Memory;

using TesoraExchange;

using NFTApplication.Extentions;



namespace NFTApplication.Utility
{
    public interface ICurrencyUtility
    {
        string CurencyStringFormat(decimal amount, string formatId, string suffix);
        string FiatStringFormat(decimal amount, string formatId, string suffix = "");
        Task<string> CurrencyConversion(decimal amount, string currency, string displayCurrency);
    }



    /// <summary>
    /// Currency Utility
    /// </summary>
    public class CurrencyUtility : ICurrencyUtility
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        private readonly IMemoryCache _memoryCache;
        private static readonly SemaphoreSlim _semaphore = new(1,1);


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="exchangeRateProvider"></param>
        /// <param name="memoryCache"></param>
        public CurrencyUtility(IExchangeRateProvider exchangeRateProvider, IMemoryCache memoryCache)
        {
            _exchangeRateProvider = exchangeRateProvider;
            _memoryCache = memoryCache ?? throw new ArgumentException("Memory Cache not configured");
        }



        /// <summary>
        /// For Fiat Currency, what is the culture so we can format the amount
        /// </summary>
        /// <param name="formatId"></param>
        /// <returns></returns>
        private static CultureInfo GetCurrencyCultureInfo(string formatId)
        {
            var  culture = formatId switch
            {
                "EUR" => "es-ES",
                "USD" => "en-US",
                _ => "en-US",
            };

            return new CultureInfo(culture);
        }

        /// <summary>
        /// Format currency with suffix
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="formatId"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public string CurencyStringFormat(decimal amount, string formatId, string suffix)
        {
            CultureInfo cultureInfo = GetCurrencyCultureInfo(formatId);
            NumberFormatInfo nfi = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "";

            if (suffix == "USD" || suffix == "EUR")
                return amount.TruncateEx(2).ToString("C", nfi).Trim() + " " + suffix;
            else
                return amount.ToString("0.00000000", nfi) + " " + suffix;
        }


        /// <summary>
        /// Format Fiat amount 
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="formatId"></param>
        /// <param name="suffix"></param>
        /// <returns></returns>
        public string FiatStringFormat(decimal amount, string formatId, string suffix = "")
        {
            CultureInfo cultureInfo = GetCurrencyCultureInfo(formatId);
            NumberFormatInfo nfi = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "";

            return amount.TruncateEx(2).ToString("C", nfi).Trim() + " " + suffix;
        }

        /// <summary>
        /// Format Crypto amount
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="formatId"></param>
        /// <returns></returns>
        public string CryptoStringFormat(decimal amount, string formatId)
        {
            CultureInfo cultureInfo = GetCurrencyCultureInfo(formatId);
            NumberFormatInfo nfi = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
            nfi = (NumberFormatInfo)nfi.Clone();
            nfi.CurrencySymbol = "";

            return amount.ToString("0.00000000", nfi);
        }

        /// <summary>
        /// Currency Conversion
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <param name="displayCurrency"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> CurrencyConversion(decimal amount, string currency, string displayCurrency)
        {
            decimal rate = 1.00m;
            if (currency != displayCurrency)
            {
                var pair = $"{currency}:{displayCurrency}";
                if (_memoryCache.TryGetValue(pair, out decimal cacheRate))
                {
                    rate = cacheRate;
                }
                else
                {
                    try
                    {
                        await _semaphore.WaitAsync();

                        // Now that we wanted our turn, did someone else set it?
                        if (_memoryCache.TryGetValue(pair, out cacheRate))
                            rate = cacheRate;
                        else
                        {
                            // Get the exchange rates for the given currency, the one the amount is in
                            var ratesItem = await _exchangeRateProvider.GetExchangeRateAsync(currency, null);

                            // Find the exchange rate for the display currency, the one that amount need to be converted to
                            var rateDisplay = ratesItem.FirstOrDefault(x => x.Ticker == displayCurrency);
                            if (rateDisplay == null)
                                throw new Exception($"Unable to determine the exchange rate for {displayCurrency}");

                            // Set the rate we retrieved
                            rate = rateDisplay.Rate;

                            var cacheEntryOptions = new MemoryCacheEntryOptions()
                                                        .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                                                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(15))
                                                        .SetPriority(CacheItemPriority.Normal)
                                                        .SetSize(32);

                            // Save rate in cache
                            _memoryCache.Set(pair, rate, cacheEntryOptions);
                        }
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                }

            }


            var decimalPlaces = displayCurrency switch
            {
                "USD" => 2,
                "EUR" => 2,
                _ => 8
            };

            var convertedPrice = Math.Round(amount * rate, decimalPlaces, MidpointRounding.AwayFromZero);

            return CurencyStringFormat(convertedPrice, displayCurrency, displayCurrency);
        }
    }
}

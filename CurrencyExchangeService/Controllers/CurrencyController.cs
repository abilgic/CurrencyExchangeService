using CurrencyExchangeService.Models;
using Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CurrencyExchangeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _config;
        private readonly ILogger<CurrencyController> _logger;
        public CurrencyController(ICurrencyService currencyService, IConfiguration config, ILogger<CurrencyController> logger, IMemoryCache memoryCache)
        {
            _currencyService = currencyService;
            _memoryCache = memoryCache;
            _config = config;
            _logger = logger;
        }

        [HttpGet("GetConversion")]
        public double GetConversion( string currency1, string currency2, double amount)
        {
            _logger.LogInformation("GetConversion execution started...");
            string requestUrl = $"{Constants.BaseUrl}convert";
            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = $"apikey={_config.GetValue<string>("ApiValues:ApiKey2")}&to={currency2}&from={currency1}&amount={amount}";// m0PfB2FUoQw6jqOOOQrvvrKEqzvGWF0R";

            var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
            HttpClient _httpClient = new HttpClient();
            var response = _httpClient.Send(httpRequestMessage);
            var resultstr = response.Content.ReadAsStringAsync().Result;
            var resultobject = JsonConvert.DeserializeObject<ConvertResponse>(resultstr);
            
            double resultvalue = 0;
            if (resultobject != null)
            {
                resultvalue = resultobject.result;
                var model = new Data.Models.ConversionLog()
                {
                    Createdate = DateTime.Now,
                    Currency1 = currency1,
                    Currency2 = currency2,
                    Result = resultvalue,
                    Amount = amount
                };
                var saveresult = _currencyService.Add(model).Result;
                var logresult = saveresult > 0 ? "Inserted to table" : "Not Inserted table";
                _logger.LogInformation($"GetConversion {logresult}");
            }
            _logger.LogInformation("GetConversion execution finished...");
            return resultvalue;
        }

        [HttpGet("GetLatest")]
        public Rates GetLatest([FromQuery] string symbols, string basevalue)
       {
            _logger.LogInformation("GetLatest execution started...");
            string requestUrl = $"{Constants.BaseUrl}latest";
            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = $"apikey={_config.GetValue<string>("ApiValues:ApiKey2")}&symbols={symbols}&base={basevalue}";

            var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
            HttpClient _httpClient = new HttpClient();
            var response = _httpClient.Send(httpRequestMessage);
            var resultstr = response.Content.ReadAsStringAsync().Result;
            var resultobject = JsonConvert.DeserializeObject<LatestResponse>(resultstr);
            
            Rates resultvalue = new Rates();
            if (resultobject != null)
            {
                resultvalue = resultobject.rates;
            }
            _logger.LogInformation("GetLatest execution finished...");

            return resultvalue;
        }

        //[HttpGet("GetSymbols")]
        //public Symbols GetSymbols()
        //{
        //    _logger.LogInformation("GetSymbols execution started...");
        //    string requestUrl = $"{Constants.BaseUrl}symbols";
        //    UriBuilder builder = new UriBuilder(requestUrl);
        //    builder.Query = $"apikey={_config.GetValue<string>("ApiValues:ApiKey2")}";

        //    var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
        //    HttpClient _httpClient = new HttpClient();
        //    var response = _httpClient.Send(httpRequestMessage);
        //    var resultstr = response.Content.ReadAsStringAsync().Result;
        //    var resultobject = JsonConvert.DeserializeObject<SymbolResponse>(resultstr);
            
        //   var resultvalue = new Symbols();
        //    if (resultobject != null)
        //    {
        //        resultvalue =  resultobject.symbols;
        //    }
        //    _logger.LogInformation("GetSymbols execution finished...");

        //    return resultvalue ?? new Symbols();
        //}

        [HttpGet("GetSymbolList")]
        public Symbols GetSymbolList()
        {
            _logger.LogInformation("GetSymbols execution started...");
            var cacheKey = "symbolList";
            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out Symbols symbolList))
            {   //Get data from server                                
                string requestUrl = $"{Constants.BaseUrl}symbols";
                UriBuilder builder = new UriBuilder(requestUrl);
                builder.Query = $"apikey={_config.GetValue<string>("ApiValues:ApiKey2")}";

                var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
                HttpClient _httpClient = new HttpClient();
                var response = _httpClient.Send(httpRequestMessage);
                var resultstr = response.Content.ReadAsStringAsync().Result;
                var resultobject = JsonConvert.DeserializeObject<SymbolResponse>(resultstr);
               
                if (resultobject != null)
                {
                    symbolList = resultobject.symbols;
                }
               
                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                //setting cache entries
                _memoryCache.Set(cacheKey, symbolList, cacheExpiryOptions);
            }
            _logger.LogInformation("GetSymbols execution finished...");
            return symbolList;
        }
    }
}



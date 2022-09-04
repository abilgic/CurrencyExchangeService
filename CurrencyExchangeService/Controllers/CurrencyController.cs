using CurrencyExchangeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace CurrencyExchangeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CurrencyController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("GetConversion")]
        public double GetConversion( string currency1, string currency2, double amount)
        {           
           
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
            }

            return resultvalue;
        }
        [HttpGet("GetLatest")]
        public Rates GetLatest([FromQuery] string symbols, string basevalue)
       { 
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

            return resultvalue;
        }

        [HttpGet("GetSymbols")]
        public Symbols GetSymbols()
        {            
            string requestUrl = $"{Constants.BaseUrl}symbols";
            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = $"apikey={_config.GetValue<string>("ApiValues:ApiKey2")}";

            var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
            HttpClient _httpClient = new HttpClient();
            var response = _httpClient.Send(httpRequestMessage);
            var resultstr = response.Content.ReadAsStringAsync().Result;
            var resultobject = JsonConvert.DeserializeObject<SymbolResponse>(resultstr);
            
           var resultvalue = new Symbols();
            if (resultobject != null)
            {
                resultvalue =  resultobject.symbols;
            }

            return resultvalue ?? new Symbols();
        }

    }

}



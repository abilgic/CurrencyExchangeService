using CurrencyExchangeService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet(Name = "Convert")]
        public int GetConversion(int c1)
        {
            var currency1 = "USD";
            var currency2 = "EUR";
            var amount = 10;
            // string requestUrl = $"https://api.apilayer.com/exchangerates_data/convert?apikey=m0PfB2FUoQw6jqOOOQrvvrKEqzvGWF0R&to={currency2}&from={currency1}&amount={amount}";
            string requestUrl = $"{Constants.BaseUrl}convert";//?apikey=m0PfB2FUoQw6jqOOOQrvvrKEqzvGWF0R&to={currency2}&from={currency1}&amount={amount}";
            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = $"apikey={_config.GetValue<string>("ApiValues:ApiKey2")}&to={currency2}&from={currency1}&amount={amount}";// m0PfB2FUoQw6jqOOOQrvvrKEqzvGWF0R";
            var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
            HttpClient _httpClient = new HttpClient();
            var result = _httpClient.Send(httpRequestMessage);
            using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            return 0;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;

namespace CurrencyExchangeService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var currency1 = "USD";
            var currency2 = "EUR";
            var amount = 10;
            string requestUrl = $"https://api.apilayer.com/exchangerates_data/convert?apikey=m0PfB2FUoQw6jqOOOQrvvrKEqzvGWF0R&to={currency2}&from={currency1}&amount={amount}";
            //var requestUrl = "https://api.apilayer.com/exchangerates_data/convert?to=USD&from=EUR&amount=10";

            //var requestUrl = "https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100&2022-02-22";
            // var requestUrl = "https://api.apilayer.com/exchangerates_data/symbols";
            //var requestUrl = "https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100&2022-02-22";
            //var requestUrl = "https://api.apilayer.com/exchangerates_data/symbols";
            UriBuilder builder = new UriBuilder(requestUrl);
            //builder.Query = "apikey=iZxJYD0qS1fvhajwJGl2XDFtVHuZYPZm";

            var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = builder.Uri };
            //var httpRequestMessage = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(requestUrl)  };


            HttpClient _httpClient = new HttpClient();
            var result = _httpClient.Send(httpRequestMessage);
            using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
            {
                Console.WriteLine(sr.ReadToEnd());
            }

            //_httpClient.BaseAddress = new Uri("https://data.fixer.io/api/");
            //var result = _httpClient.GetAsync("symbols?access_key=iZxJYD0qS1fvhajwJGl2XDFtVHuZYPZm").Result;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
                .ToArray();
        }/*
            int amount = 10;
            string from = "EUR";
            string to = "USD";
            
            var client = new RestClient("https://data.fixer.io/api/symbols?access_key=iZxJYD0qS1fvhajwJGl2XDFtVHuZYPZm");
           // client.Timeout = -1;

            var request = new RestRequest(Method.Get.ToString());
            //request.AddHeader("apikey", "iZxJYD0qS1fvhajwJGl2XDFtVHuZYPZm");

            var response = client.Execute(request);
            Console.WriteLine(response.Content);

            ////var client = new RestClient("https://api.apilayer.com/fixer/2022-09-03?symbols=symbols&base=base");
            //var client = new RestClient("https://api.apilayer.com/fixer/symbols");
            ////var client = new RestClient("https://api.apilayer.com/fixer/date?symbols=symbols&base=base");
            ////var client = new RestClient("https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100&2022-02-22");
            ////client.Timeout = -1;
            ////var obj = new jsonmodel { date = DateTime.Now };
            //var request = new RestRequest(Method.Get.ToString());
            //request.AddHeader("apikey", "iZxJYD0qS1fvhajwJGl2XDFtVHuZYPZm");
            ////request.AddParameter("date", "2022-09-03", ParameterType.RequestBody,true);
            ////request.AddJsonBody(obj);
            //var response = client.Execute(request);

            //Console.WriteLine(response.Content);
        */

    }

    public class jsonmodel
    {
        public DateTime date { get; set; }
    }
}
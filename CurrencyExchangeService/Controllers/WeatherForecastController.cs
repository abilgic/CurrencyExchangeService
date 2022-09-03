using Microsoft.AspNetCore.Mvc;
using RestSharp;

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
            var client = new RestClient("https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100");

            RestClient client = new RestClient("https://api.apilayer.com/fixer/convert?to=USD&from=EUR&amount=100");
            //client.Timeout = -1;

            var request = new RestRequest("GET");
            request.AddHeader("apikey", "iZxJYD0qS1fvhajwJGl2XDFtVHuZYPZm");

            var response =client.Execute(request);
            Console.WriteLine(response.Content.ToString());

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
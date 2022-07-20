using System.Globalization;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WorkFlowManager.Service.Models.Dto;

namespace WorkFlowManager.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
                .ToArray();
        }

        [HttpGet("SendMessage")]
        public async Task<IActionResult> SendMessage()
        {
            await _publishEndpoint.Publish<MethodResult>(
                MethodResult.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            return Ok();
        }
    }
}
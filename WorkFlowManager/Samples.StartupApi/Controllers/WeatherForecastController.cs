using Microsoft.AspNetCore.Mvc;
using Samples.ShoppigCard;
using WorkFlowManager.Core.Interfaces;

namespace Samples.StartupApi.Controllers
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
        private readonly IWfManager wfManager;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWfManager wfManager)
        {
            _logger = logger;
            this.wfManager = wfManager;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            WorkFlowBiz wfBiz = new WorkFlowBiz(wfManager);
            wfBiz.CreateWorkFlow();



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
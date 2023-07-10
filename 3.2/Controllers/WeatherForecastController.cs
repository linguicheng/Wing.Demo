using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _3._2.Controllers
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

        [HttpGet("Breaker1")]
        public string Breaker1()
        {
            Thread.Sleep(120 * 1000);
            return "我是Breaker1";
        }
        [HttpGet("Breaker2/{name}")]
        public string Breaker2(string name)
        {
            Thread.Sleep(120 * 1000);
            return $"我是Breaker2，name：{name}";
        }
    }
}

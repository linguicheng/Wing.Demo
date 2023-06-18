using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Wing;
using Wing.ServiceProvider;

namespace _2._2.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IServiceFactory _serviceFactory;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IServiceFactory serviceFactory,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _serviceFactory = serviceFactory;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        public string Test()
        {
            return App.Configuration["Test"];
        }
        private static int count1 = 0;
        private static int count2 = 0;
        private static int count3 = 0;
        [HttpGet]
        public void LoadBalance()
        {
            Parallel.For(0, 10, async x =>
            {
                await _serviceFactory.InvokeAsync("Wing.Demo_2.3", async serviceAddr =>
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(serviceAddr.ToString());
                    var response = await client.GetAsync("/WeatherForecast/Test");
                    if (serviceAddr.Port == 2311)
                    {
                        Interlocked.Increment(ref count1);
                    }
                    else if (serviceAddr.Port == 2312)
                    {
                        Interlocked.Increment(ref count2);
                    }
                    else
                    {
                        Interlocked.Increment(ref count3);
                    }
                    _logger.LogInformation($"count1:{count1}，count2：{count2}，count3：{count3}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogInformation($"第{x}次请求，结果：{await response.Content.ReadAsStringAsync()}");
                    }
                    else
                    {
                        _logger.LogInformation($"第{x}次请求，状态码：{response.StatusCode}");
                    }
                });

            });
        }
    }
}

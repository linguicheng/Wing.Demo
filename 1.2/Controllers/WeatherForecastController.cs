using _1._4;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Wing.ServiceProvider;

namespace _1._2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IServiceFactory _serviceFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IServiceFactory serviceFactory, IHttpClientFactory httpClientFactory, ILogger<WeatherForecastController> logger)
        {
            _serviceFactory = serviceFactory;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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

        [HttpGet("hello")]
        public Task<string> SayHello()
        {
            return _serviceFactory.InvokeAsync("Wing.Demo_1.4", async serviceAddr =>
            {
                var channel = GrpcChannel.ForAddress(serviceAddr.ToString());
                var greeterClient = new Greeter.GreeterClient(channel);
                var result = await greeterClient.SayHelloAsync(new HelloRequest { Name = "Wing" });
                return result.Message;
            });
        }

        [HttpGet("LoadBalance")]
        public void LoadBalance()
        {
            Parallel.For(0, 10, async x =>
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("http://localhost:2210");
                var response = await client.GetAsync("/WeatherForecast/LoadBalance");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"第{x}次请求，结果：{await response.Content.ReadAsStringAsync()}");
                }
                else
                {
                    _logger.LogInformation($"第{x}次请求，状态码：{response.StatusCode}");
                }
                
            });

        }
    }
}

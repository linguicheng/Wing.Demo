using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Sample.Auth;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Wing.ServiceProvider;

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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly IAuth _auth;
        private readonly HttpContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IHttpClientFactory httpClientFactory,
            IServiceFactory serviceFactory, 
            IAuth auth,
            IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _serviceFactory = serviceFactory;
            _auth = auth;
            _context = contextAccessor.HttpContext;
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

        [HttpGet("Timeout")]
        public string Timeout()
        {
            Thread.Sleep(120 * 1000);
            return "我是Timeout";
        }

        [HttpGet("Retry")]
        public string Retry()
        {
            Thread.Sleep(5 * 1000);
            return "我是Retry";
        }

        [HttpGet("RateLimit")]
        public string RateLimit()
        {
            return "我是RateLimit";
        }

        [HttpGet("BulkHead")]
        public string BulkHead()
        {
            return "我是BulkHead";
        }

        [HttpGet("BulkHeadTest")]
        public void BulkHeadTest()
        {
            Parallel.For(0, 20, async x =>
            {
                await _serviceFactory.InvokeAsync("Wing.Demo_3.2.5", async serviceAddr =>
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri(serviceAddr.ToString());
                    var response = await client.GetAsync("Wing.Demo_3.2/weatherforecast/BulkHead");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        _logger.LogInformation($"第{x + 1}次请求，结果：{await response.Content.ReadAsStringAsync()}");
                    }
                    else
                    {
                        _logger.LogInformation($"第{x + 1}次请求，状态码：{response.StatusCode}");
                    }
                });
            });
        }

        [HttpGet("JwtAuth")]
        public string JwtAuth()
        {
            return "我是JwtAuth";
        }


        [HttpGet("JwtAuthTest")]
        public Task<string> JwtAuthTest()
        {
           return _serviceFactory.InvokeAsync("Wing.Demo_3.2.6", async serviceAddr =>
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(serviceAddr.ToString());
                var token = _auth.GetToken();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.GetStringAsync("Wing.Demo_3.2/weatherforecast/JwtAuth");
            });
        }

        [HttpGet("GetToken")]
        public string GetToken()
        {
            return _auth.GetToken("Wing");
        }

        [HttpGet("AuthKey")]
        public string AuthKey()
        {
            return "我是AuthKey";
        }

        [HttpGet("AuthKeyTest")]
        public Task<string> AuthKeyTest()
        {
            return _serviceFactory.InvokeAsync("Wing.Demo_3.2.6", async serviceAddr =>
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(serviceAddr.ToString());
                //client.DefaultRequestHeaders.Add("AuthKey", "abcd");
                return await client.GetStringAsync("Wing.Demo_3.2/weatherforecast/AuthKey");
            });
        }

        [HttpGet("CustomRoute/{name}")]
        public string CustomRoute(string name)
        {
            return $"自定义路由测试：{name}";
        }

        [HttpGet("Aggregation1")]
        public string Aggregation1()
        {
            return $"聚合服务测试1";
        }

        [HttpGet("Aggregation2")]
        public string Aggregation2()
        {
            return $"聚合服务测试2";
        }

        [HttpGet("JwtAuthPolicy")]
        public string JwtAuthPolicy()
        {
            return $"我是JwtAuthPolicy，网关转发的请求头user-account:{_context.Request.Headers["user-account"]}";
        }
    }
}

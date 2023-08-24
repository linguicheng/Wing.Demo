using _5._2.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace _5._2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFreeSqlDemoService _freeSqlDemoService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFreeSqlDemoService freeSqlDemoService)
        {
            _logger = logger;
            _freeSqlDemoService = freeSqlDemoService;
        }

        [HttpGet]
        public Task<int> Get(string name="FreeSql")
        {
            return _freeSqlDemoService.Add(new Entity.FreeSqlDemo { Id = Guid.NewGuid().ToString(), Name = name });
        }
    }
}

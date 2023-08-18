using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wing.Persistence.Saga;
using Wing.Saga.Client;

namespace _4._2_1
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get(string name)
        {
            Saga.Start("Saga-Wing.Demo_4.2.1", new SagaOptions { TranPolicy = TranPolicy.Backward })
                 .Then(new SampleSagaUnit1(), new SampleUnitModel { Name = "事务单元1", HelloName = name })
                 .Then(new SampleSagaUnit2(), new SampleUnitModel { Name = "事务单元2", HelloName = name })
                 .Then(new SampleSagaUnit3(), new SampleUnitModel { Name = "事务单元3", HelloName = name })
                 .End();
            return $"Hello {name}";
        }
    }
}

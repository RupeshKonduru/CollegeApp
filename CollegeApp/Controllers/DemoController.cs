using CollegeApp.Logger;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/Demo")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;
        public DemoController(ILogger<DemoController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogTrace("Logg Trace from Index Method");
            _logger.LogDebug("Logg Debug from Index Method");
            _logger.LogInformation("Logg Information from Index Method");
            _logger.LogWarning("Logg Warning from Index Method");
            _logger.LogError("Logg Error from Index Method");
            _logger.LogCritical("Logg Critical from Index Method");
            return Ok();
        }
    }
}

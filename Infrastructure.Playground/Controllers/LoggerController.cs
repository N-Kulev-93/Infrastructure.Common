using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Playground.Controllers
{
    public class SimpleMessageModel
    {
        public string Message { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        readonly ILogger<LoggerController> _logger;

        public LoggerController(ILogger<LoggerController> logger)
        {
            _logger = logger;
        }

        [HttpPost("log-message")]
        public ActionResult LogSimpleMessage([FromBody] SimpleMessageModel request)
        {
            _logger.LogInformation(request.Message);

            return Ok();
        }
    }
}

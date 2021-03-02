using Microsoft.AspNetCore.Mvc;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("health")]
  [ApiController]
  public class HealthController : ControllerBase
  {
    [HttpGet]
    public ActionResult Get()
    {
      return Ok();
    }
  }
}
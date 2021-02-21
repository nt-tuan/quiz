using Microsoft.AspNetCore.Mvc;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("health")]
  [ApiController]
  public class Healthontroller : ControllerBase
  {
    [HttpGet]
    public ActionResult Get()
    {
      return Ok();
    }
  }
}
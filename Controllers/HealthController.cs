using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Quiz.Data;
using ThanhTuan.Quiz.Entities;

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
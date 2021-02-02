using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Quiz.Data;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("")]
  [ApiController]
  public class Healthontroller : ControllerBase
  {
    public ActionResult Get()
    {
      return Ok();
    }
  }
}
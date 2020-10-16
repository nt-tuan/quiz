using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class AuthorizeController : ControllerBase
  {
    public ActionResult Get()
    {
      return Ok();
    }
  }
}
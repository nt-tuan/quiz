using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Web.Api
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class AuthorizeController : ControllerBase
  {
    public ActionResult Get(string requiredRole)
    {
      var roleClaims = User.FindAll(ClaimTypes.Role);      
      foreach(var claim in roleClaims) {
        if (claim.Value == requiredRole) return Ok();
      }
      return Unauthorized();
    }
  }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThanhTuan.Quiz.Controllers.Models;
using ThanhTuan.Quiz.Services;

namespace WebApi.Controllers
{
  [ApiController]
  [Route("api/authentication")]
  public class AuthenticationController : ControllerBase
  {
    private IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest model)
    {
      var response = await _userService.Authenticate(model);

      if (response == null)
        return BadRequest(new { message = "Username or password is incorrect" });

      return Ok(response);
    }
  }
}
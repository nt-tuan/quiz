
using System.Linq;
using System.Threading.Tasks;
using dmc_auth;
using dmc_auth.Controllers.Models;
using dmc_auth.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
  [Route("api/[controller]")]
  [ApiController]
  public class MeController : ControllerBase
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AdminController> _logger;

    public MeController(UserManager<ApplicationUser> userManager, ILogger<AdminController> logger)
    {
      _userManager = userManager;
      _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<UserResponse>> Get()
    {
      var appUser = await GetUser();
      if (appUser == null) return NotFound();
      var roles = await _userManager.GetRolesAsync(appUser);
      return new UserResponse(appUser, roles);
    }

    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
      var user = await GetUser();
      if (user == null)
        return NotFound();
      var rs = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
      if (rs.Succeeded)
        return Ok();
      return ResponseIdentityResultError(rs);
    }

    private async Task<ApplicationUser> GetUser()
    {
      foreach (var p in Request.Headers)
      {
        _logger.LogInformation("HEADERS -- {0}:{1}", p.Key, p.Value);
      }
      return await _userManager.FindByNameAsync(Request.Headers[Constant.USER_HEADER_KEY]);
    }

    private BadRequestObjectResult ResponseIdentityResultError(IdentityResult identityResult)
    {
      var messages = identityResult.Errors.Select(u => $"{u.Code}: {u.Description}").ToList();
      var errResponse = new ErrorResponse();
      if (messages.Count > 0)
        errResponse.Message = messages[0];
      errResponse.Messages = messages;
      return BadRequest(errResponse);
    }

  }
}
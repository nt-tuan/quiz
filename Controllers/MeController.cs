using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dmc_auth;
using dmc_auth.Controllers.Models;
using dmc_auth.Data;
using dmc_auth.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CleanArchitecture.Web.Api
{
  [Route("api/[controller]")]
  [ApiController]
  [Authorize]
  public class MeController : ControllerBase
  {
    //private readonly ApplicationDbContext _context;
    private readonly SignInManager<ApplicationUser> _signinManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<AdminController> _logger;

    public MeController(SignInManager<ApplicationUser> signinManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AdminController> logger)
    {
      _signinManager = signinManager;
      _userManager = userManager;
      _roleManager = roleManager;
      _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<UserResponse>> Get()
    {
      var appUser = await _userManager.GetUserAsync(User);
      if (appUser == null) return NotFound();
      var roles = await _userManager.GetRolesAsync(appUser);
      return new UserResponse(appUser, roles);
    }

    [HttpPost("changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
      _logger.LogInformation(JsonConvert.SerializeObject(
        ((ClaimsIdentity)User.Identity).Claims.Select(claim => new { claim.Type, claim.Value, claim.ValueType })));
      var user = await _userManager.FindByNameAsync(User.Identity.Name);
      if (user == null)
        return NotFound();
      var rs = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
      if (rs.Succeeded)
        return Ok();
      return responseIdentityResultError(rs);
    }

    private BadRequestObjectResult responseIdentityResultError(IdentityResult identityResult)
    {
      var messages = identityResult.Errors.Select(u => $"{u.Code}: {u.Description}").ToList();
      var errResponse = new ErrorResponse();
      if (messages.Count > 0)
        errResponse.message = messages[0];
      errResponse.messages = messages;
      return BadRequest(errResponse);
    }

  }
}

using System.Linq;
using System.Text;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using dmc_auth.Data;
using dmc_auth.Entities;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using dmc_auth.Controllers.Models;
using dmc_auth.Hydra;
using dmc_auth.Hydra.Models;
using Microsoft.AspNetCore.Http;

namespace dmc_auth.Controllers
{
  [Route("api")]
  [ApiController]
  public class IDPController : ControllerBase
  {
    ILogger<IDPController> _logger;
    ApplicationDbContext _context;
    UserManager<ApplicationUser> _userManager;
    IHydra _hydra;
    public IDPController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
    IHydra hydra,
    ILogger<IDPController> logger)
    {
      _logger = logger;
      _context = context;
      _userManager = userManager;
      _hydra = hydra;
    }

    [HttpGet]
    [Route("health")]
    public ActionResult Health()
    {
      return Ok();
    }

    [HttpGet]
    [Route("login")]
    public async Task<ActionResult<LoginInfo>> GetLoginInfo(string login_challenge)
    {
      var loginInfo = await _hydra.GetLoginInfo(login_challenge);
      if (loginInfo.skip)
      {
        var user = await _userManager.FindByIdAsync(loginInfo.subject);
        if (user == null) return BadRequest(IDPErrors.UserNotFound);
        loginInfo.username = user.UserName;
        return Ok(loginInfo);
      }
      return Ok(loginInfo);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AcceptLoginResponse>> Login(Login model)
    {
      var loginInfo = await _hydra.GetLoginInfo(model.login_challenge);
      if (loginInfo.skip)
      {
        return await _hydra.AcceptLogin(new AcceptLoginRequest(loginInfo.subject), model.login_challenge);
      }
      var appuser = await _userManager.FindByEmailAsync(model.username);
      if (appuser == null)
      {
        appuser = await _userManager.FindByNameAsync(model.username);
      }
      if (appuser == null)
      {
        return NotFound();
      }
      var valid = await _userManager.CheckPasswordAsync(appuser, model.password);
      if (!valid)
      {
        return BadRequest(IDPErrors.InvalidCredential);
      }
      return await _hydra.AcceptLogin(new AcceptLoginRequest(appuser.Id), model.login_challenge);
    }


    [HttpPost]
    [Route("consent/accept")]
    public async Task<ActionResult<AcceptConsentResponse>> Consent(string consent_challenge)
    {
      var consent = await _hydra.GetConsentInfo(consent_challenge);
      var user = await _userManager.FindByIdAsync(consent.subject);
      if (user == null) return BadRequest(IDPErrors.UserNotFound);
      var roles = await _userManager.GetRolesAsync(user);
      // var roles = new[] { "user.admin", "user.get" };
      var authURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent/accept?consent_challenge={consent_challenge}";
      var requestContent = new AcceptConsentRequest(consent, roles.ToArray(), user);
      var acceptResponse = await _hydra.AcceptConsent(requestContent, consent_challenge);
      return acceptResponse;
    }


    [HttpPost]
    [Route("logout/accept")]
    public async Task<ActionResult<AcceptLogoutResponse>> Logout(string logout_challenge)
    {
      var acceptResponse = await _hydra.AcceptLogout(logout_challenge);
      return acceptResponse;
    }
  }
}

using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using dmc_auth.Entities;
using dmc_auth.Controllers.Models;
using dmc_auth.Hydra;
using dmc_auth.Hydra.Models;

namespace dmc_auth.Controllers
{
  [Route("api")]
  [ApiController]
  public class IDPController : ControllerBase
  {
    readonly UserManager<ApplicationUser> _userManager;
    readonly IHydra _hydra;
    public IDPController(UserManager<ApplicationUser> userManager,
    IHydra hydra)
    {
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
      if (loginInfo.Skip)
      {
        var user = await _userManager.FindByIdAsync(loginInfo.Subject);
        if (user == null) return BadRequest(IDPErrors.UserNotFound);
        loginInfo.Username = user.UserName;
        return Ok(loginInfo);
      }
      return Ok(loginInfo);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AcceptLoginResponse>> Login(Login model)
    {
      var loginInfo = await _hydra.GetLoginInfo(model.LoginChallenge);
      if (loginInfo.Skip)
      {
        return await _hydra.AcceptLogin(new AcceptLoginRequest(loginInfo.Subject), model.LoginChallenge);
      }
      var appuser = await _userManager.FindByEmailAsync(model.Username);
      if (appuser == null)
      {
        appuser = await _userManager.FindByNameAsync(model.Username);
      }
      if (appuser == null)
      {
        return NotFound();
      }
      var valid = await _userManager.CheckPasswordAsync(appuser, model.Password);
      if (!valid)
      {
        return BadRequest(IDPErrors.InvalidCredential);
      }
      return await _hydra.AcceptLogin(new AcceptLoginRequest(appuser.Id), model.LoginChallenge);
    }


    [HttpPost]
    [Route("consent/accept")]
    public async Task<ActionResult<AcceptConsentResponse>> Consent(string consent_challenge)
    {
      var consent = await _hydra.GetConsentInfo(consent_challenge);
      var user = await _userManager.FindByIdAsync(consent.Subject);
      if (user == null) return BadRequest(IDPErrors.UserNotFound);
      var roles = await _userManager.GetRolesAsync(user);
      // var roles = new[] { "user.admin", "user.get" };      
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
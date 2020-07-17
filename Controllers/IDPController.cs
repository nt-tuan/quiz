
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
using dmc_auth.Models;
using Newtonsoft.Json;

namespace dmc_auth.Controllers
{
  [Route("api")]
  [ApiController]
  public class IDPController : ControllerBase
  {

    ApplicationDbContext _context;
    UserManager<ApplicationUser> _userManager;
    public IDPController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
      _context = context;
      _userManager = userManager;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login(LoginModel model)
    {

      // var appuser = await _userManager.FindByEmailAsync(model.username);
      // if (appuser == null)
      // {
      //   appuser = await _userManager.FindByNameAsync(model.username);
      // }
      // //If wrong user
      // if (appuser == null)
      // {
      //   return NotFound();
      // }
      // var valid = await _userManager.CheckPasswordAsync(appuser, model.password);
      var valid = true;
      if (!valid)
      {
        return BadRequest(new
        {
          message = "invalid_credential"
        });
      }
      return await acceptLogin(model.login_challenge, model.username);
    }


    [HttpPost]
    [Route("consent/accept")]
    public async Task<ActionResult> Consent(string consent_challenge)
    {
      var authURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent/accept?consent_challenge={consent_challenge}";
      var client = new HttpClient();
      var stringRequestContent = new StringContent("{}", Encoding.UTF8, "application/json");
      var response = await client.PutAsync(authURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonConvert.DeserializeObject<AcceptConsentResponse>(stringContent);
        return Redirect(acceptResponse.redirect_to);
      }
      return BadRequest(new
      {
        message = stringContent
      });
    }

    private async Task<ActionResult> acceptLogin(string login_challenge, string username)
    {
      var client = new HttpClient();
      var acceptURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login/accept?login_challenge={login_challenge}";
      var requestContent = new AcceptLoginRequest(username);
      var stringRequestContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
      var response = await client.PutAsync(acceptURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonConvert.DeserializeObject<AcceptLoginResponse>(stringContent);
        return Redirect(acceptResponse.redirect_to);
      }
      return BadRequest(new
      {
        message = stringContent
      });
    }
  }
}
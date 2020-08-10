
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

namespace dmc_auth.Controllers
{
    [Route("api")]
    [ApiController]
    public class IDPController : ControllerBase
    {
        ILogger<IDPController> _logger;
        ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        public IDPController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<IDPController> logger)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("health")]
        public ActionResult Health()
        {
            return Ok();
        }

        [HttpGet]
        [Route("login")]
        public async Task<ActionResult<LoginInformationResponse>> GetLoginInfo(string login_challenge)
        {
            var url = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login?login_challenge={login_challenge}";
            var httpClientHandler = getIgnoreSSLHandler();
            var httpClient = new HttpClient(httpClientHandler);
            var responseMessage = await httpClient.GetAsync(url);
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseText = await responseMessage.Content.ReadAsStringAsync();
                var loginInfo = JsonConvert.DeserializeObject<LoginInformationResponse>(responseText);
                return Ok(loginInfo);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AcceptLoginResponse>> Login(LoginModel model)
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
                return BadRequest(IDPErrors.InvalidCredential);
            }
            return await acceptLogin(model.login_challenge, model.username);
        }


        [HttpPost]
        [Route("consent/accept")]
        public async Task<ActionResult<AcceptConsentResponse>> Consent(string consent_challenge)
        {
            var httpClientHandler = getIgnoreSSLHandler();
            var consent = await getConsentInformation(consent_challenge);
            var user = await _userManager.FindByNameAsync(consent.subject);
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(consent.subject);
            }
            if (user == null) return BadRequest(IDPErrors.UserNotFound);
            var roles = await _userManager.GetRolesAsync(user);
            // var roles = new[] { "user.admin", "user.get" };
            var authURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent/accept?consent_challenge={consent_challenge}";
            var client = new HttpClient(httpClientHandler);
            var requestBody = new ConsentAcceptBody(consent.requested_scope, roles.ToArray(), user.UserName);
            var requestBodyString = JsonConvert.SerializeObject(requestBody);
            var stringRequestContent = new StringContent(requestBodyString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(authURL, stringRequestContent);
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var acceptResponse = JsonConvert.DeserializeObject<AcceptConsentResponse>(stringContent);
                return Ok(acceptResponse);
            }
            return BadRequest(new ErrorResponse(stringContent));
        }


        [HttpPost]
        [Route("logout/accept")]
        public async Task<ActionResult<AcceptLogoutResponse>> Logout(string logout_challenge)
        {
            var logoutURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/logout/accept?logout_challenge={logout_challenge}";
            var client = new HttpClient(getIgnoreSSLHandler());
            var stringRequestContent = new StringContent("", Encoding.UTF8, "application/json");
            var response = await client.PutAsync(logoutURL, stringRequestContent);
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var acceptResponse = JsonConvert.DeserializeObject<AcceptLogoutResponse>(stringContent);
                _logger.LogInformation(acceptResponse.redirect_to);
                return Ok(acceptResponse);
            }
            return BadRequest(new ErrorResponse(stringContent));
        }

        private async Task<ActionResult<AcceptLoginResponse>> acceptLogin(string login_challenge, string username)
        {
            var httpClientHandler = getIgnoreSSLHandler();
            var client = new HttpClient(httpClientHandler);
            var acceptURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login/accept?login_challenge={login_challenge}";
            var requestContent = new AcceptLoginRequest(username);
            var stringRequestContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(acceptURL, stringRequestContent);
            var stringContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var acceptResponse = JsonConvert.DeserializeObject<AcceptLoginResponse>(stringContent);
                return Ok(acceptResponse);
            }
            return BadRequest(new ErrorResponse(stringContent));
        }

        private async Task<ConsentRequestBody> getConsentInformation(string challenge)
        {
            var httpClientHandler = getIgnoreSSLHandler();
            var url = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent?consent_challenge={challenge}";
            var client = new HttpClient(httpClientHandler);
            var responseMessage = await client.GetAsync(url);
            var stringContent = await responseMessage.Content.ReadAsStringAsync();
            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var consentInfo = JsonConvert.DeserializeObject<ConsentRequestBody>(stringContent);
                return consentInfo;
            }
            throw new Exception(stringContent);
        }


        HttpClientHandler getIgnoreSSLHandler()
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // DEBUGGING ONLY
            return httpClientHandler;
        }
    }
}
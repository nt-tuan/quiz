using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dmc_auth.Controllers.Models;
using dmc_auth.Entities;
using dmc_auth.Hydra.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace dmc_auth.Hydra
{
  public interface IHydra
  {
    Task<LoginInfo> GetLoginInfo(string challenge);
    Task<ConsentInfo> GetConsentInfo(string challenge);
    Task<AcceptLoginResponse> AcceptLogin(AcceptLoginRequest request, string challenge);
    Task<AcceptConsentResponse> AcceptConsent(AcceptConsentRequest requestContent, string challenge);
    Task<AcceptLogoutResponse> AcceptLogout(string challenge);
  }

  public class Hydra : IHydra
  {
    public async Task<AcceptConsentResponse> AcceptConsent(AcceptConsentRequest requestContent, string challenge)
    {
      var authURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent/accept?consent_challenge={challenge}";
      var client = getClient();
      var requestBodyString = JsonConvert.SerializeObject(requestContent);
      var stringRequestContent = new StringContent(requestBodyString, Encoding.UTF8, "application/json");
      var response = await client.PutAsync(authURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonConvert.DeserializeObject<AcceptConsentResponse>(stringContent);
        return acceptResponse;
      }
      throw new Exception(stringContent);
    }

    public async Task<AcceptLoginResponse> AcceptLogin(AcceptLoginRequest requestContent, string challenge)
    {
      var client = getClient();
      var acceptURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login/accept?login_challenge={challenge}";
      var stringRequestContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
      var response = await client.PutAsync(acceptURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonConvert.DeserializeObject<AcceptLoginResponse>(stringContent);
        return acceptResponse;
      }
      throw new Exception(stringContent);
    }

    public async Task<AcceptLogoutResponse> AcceptLogout(string challenge)
    {
      var logoutURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/logout/accept?logout_challenge={challenge}";
      var client = getClient();
      var stringRequestContent = new StringContent("", Encoding.UTF8, "application/json");
      var response = await client.PutAsync(logoutURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonConvert.DeserializeObject<AcceptLogoutResponse>(stringContent);
        return acceptResponse;
      }
      throw new Exception(stringContent);
    }

    public async Task<ConsentInfo> GetConsentInfo(string challenge)
    {
      var url = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent?consent_challenge={challenge}";
      var client = getClient();
      var responseMessage = await client.GetAsync(url);
      var stringContent = await responseMessage.Content.ReadAsStringAsync();
      if (responseMessage.StatusCode == HttpStatusCode.OK)
      {
        var consentInfo = JsonConvert.DeserializeObject<ConsentInfo>(stringContent);
        return consentInfo;
      }
      throw new Exception(stringContent);
    }

    public async Task<LoginInfo> GetLoginInfo(string challenge)
    {
      var url = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login?login_challenge={challenge}";
      var httpClient = getClient();
      var responseMessage = await httpClient.GetAsync(url);
      var responseText = await responseMessage.Content.ReadAsStringAsync();
      if (responseMessage.StatusCode != HttpStatusCode.OK)
      {
        throw new Exception(responseText);
      }
      var loginInfo = JsonConvert.DeserializeObject<LoginInfo>(responseText);
      return loginInfo;
    }

    HttpClient getClient()
    {
      var httpClientHandler = new HttpClientHandler();
      httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true; // DEBUGGING ONLY
      var client = new HttpClient(httpClientHandler);
      return client;
    }
  }
}
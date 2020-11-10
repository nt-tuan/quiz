using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using dmc_auth.Hydra.Models;
using System.Text.Json;
using System.Collections.Generic;

namespace dmc_auth.Hydra
{
  public interface IHydra
  {
    Task<LoginInfo> GetLoginInfo(string challenge);
    Task<ConsentInfo> GetConsentInfo(string challenge);
    Task<AcceptLoginResponse> AcceptLogin(AcceptLoginRequest request, string challenge);
    Task<AcceptConsentResponse> AcceptConsent(AcceptConsentRequest requestContent, string challenge);
    Task<AcceptLogoutResponse> AcceptLogout(string challenge);
    Task<TokenInstropectResponse> InstropectToken(string token, string scope);
  }

  public class Hydra : IHydra
  {
    public async Task<AcceptConsentResponse> AcceptConsent(AcceptConsentRequest requestContent, string challenge)
    {
      var authURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent/accept?consent_challenge={challenge}";
      var client = GetClient();
      var requestBodyString = JsonSerializer.Serialize(requestContent);
      var stringRequestContent = new StringContent(requestBodyString, Encoding.UTF8, "application/json");
      var response = await client.PutAsync(authURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonSerializer.Deserialize<AcceptConsentResponse>(stringContent);
        return acceptResponse;
      }
      throw new Exception(stringContent);
    }

    public async Task<AcceptLoginResponse> AcceptLogin(AcceptLoginRequest requestContent, string challenge)
    {
      var client = GetClient();
      var acceptURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login/accept?login_challenge={challenge}";
      var stringRequestContent = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
      var response = await client.PutAsync(acceptURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonSerializer.Deserialize<AcceptLoginResponse>(stringContent);
        return acceptResponse;
      }
      throw new Exception(stringContent);
    }

    public async Task<AcceptLogoutResponse> AcceptLogout(string challenge)
    {
      var logoutURL = $"{Constant.GetAuthURL()}/oauth2/auth/requests/logout/accept?logout_challenge={challenge}";
      var client = GetClient();
      var stringRequestContent = new StringContent("", Encoding.UTF8, "application/json");
      var response = await client.PutAsync(logoutURL, stringRequestContent);
      var stringContent = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        var acceptResponse = JsonSerializer.Deserialize<AcceptLogoutResponse>(stringContent);
        return acceptResponse;
      }
      throw new Exception(stringContent);
    }

    public async Task<ConsentInfo> GetConsentInfo(string challenge)
    {
      var url = $"{Constant.GetAuthURL()}/oauth2/auth/requests/consent?consent_challenge={challenge}";
      var client = GetClient();
      var responseMessage = await client.GetAsync(url);
      var stringContent = await responseMessage.Content.ReadAsStringAsync();
      if (responseMessage.StatusCode == HttpStatusCode.OK)
      {
        var consentInfo = JsonSerializer.Deserialize<ConsentInfo>(stringContent);
        return consentInfo;
      }
      throw new Exception(stringContent);
    }

    public async Task<LoginInfo> GetLoginInfo(string challenge)
    {
      var url = $"{Constant.GetAuthURL()}/oauth2/auth/requests/login?login_challenge={challenge}";
      var httpClient = GetClient();
      var responseMessage = await httpClient.GetAsync(url);
      var responseText = await responseMessage.Content.ReadAsStringAsync();
      if (responseMessage.StatusCode != HttpStatusCode.OK)
      {
        throw new Exception(responseText);
      }
      var loginInfo = JsonSerializer.Deserialize<LoginInfo>(responseText);
      return loginInfo;
    }

    public async Task<TokenInstropectResponse> InstropectToken(string token, string scope)
    {
      var url = $"{Constant.GetAuthURL()}/oauth2/introspect";
      var httpClient = GetClient();
      httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
      httpClient.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
      var formVariables = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("token", token),
                new KeyValuePair<string, string>("scope", scope)
            };
      var formContent = new FormUrlEncodedContent(formVariables);
      var response = await httpClient.PostAsync(url, formContent);
      var responseText = await response.Content.ReadAsStringAsync();
      if (response.StatusCode == HttpStatusCode.OK)
      {
        return JsonSerializer.Deserialize<TokenInstropectResponse>(responseText);
      }
      throw new Exception(responseText);
    }
    HttpClient GetClient()
    {
      var httpClientHandler = new HttpClientHandler
      {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true // DEBUGGING ONLY
      };
      var client = new HttpClient(httpClientHandler);
      return client;
    }
  }
}
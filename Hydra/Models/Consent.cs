using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using ThanhTuan.IDP.Entities;
using ThanhTuan.IDP;
namespace ThanhTuan.IDP.Hydra.Models
{
  public class ConsentAcceptAccessTokenBody
  {
  }
  public class ConsentAcceptIDTokenBody
  {
  }
  public class ConsentAcceptSessionBody
  {
  }
  public class AcceptConsentRequest
  {
    [JsonPropertyName("grant_scope")]
    public string[] GrantScope { get; set; }
    [JsonPropertyName("grant_access_token_audience")]
    public string[] GrantAccessTokenAudience { get; set; }
    [JsonPropertyName("remember")]
    public bool Remember { get; set; } = true;
    [JsonPropertyName("remember_for")]
    public ulong RememberFor { get; set; } = Constant.GetRememberDuration();
    [JsonPropertyName("session")]
    public ConsentAcceptSessionBody Session { get; set; }
    public AcceptConsentRequest(ConsentInfo consent, string[] roles, ApplicationUser user)
    {
      var grantScopes = new List<string>();
      var scopes = Constant.GetScopes();
      foreach (var scope in consent.RequestedScope)
      {
        if (scopes.Contains(scope))
        {
          grantScopes.Add(scope);
          continue;
        }
        if (roles.Contains(scope))
        {
          grantScopes.Add(scope);
        }
      }
      GrantScope = grantScopes.ToArray();
      GrantAccessTokenAudience = consent.RequestedAccessTokenAudience;
      Session = new ConsentAcceptSessionBody();
    }
  }

  public class AcceptConsentResponse
  {
    [JsonPropertyName("redirect_to")]
    public string RedirectTo { get; set; }
  }

  public class ConsentInfo
  {
    [JsonPropertyName("requested_access_token_audience")]
    public string[] RequestedAccessTokenAudience { get; set; }
    [JsonPropertyName("requested_scope")]
    public string[] RequestedScope { get; set; }
    [JsonPropertyName("subject")]
    public string Subject { get; set; }
    [JsonPropertyName("login_challenge")]
    public string LoginChallenge { get; set; }
  }

  public class RejectConsentRequest
  {
    [JsonPropertyName("error")]
    public string Error { get; set; }
    [JsonPropertyName("error_debug")]
    public string ErrorDebug { get; set; }
    [JsonPropertyName("error_description")]
    public string ErrorDescription { get; set; }
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }
  }
}
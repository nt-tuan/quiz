using System.Text.Json.Serialization;
using dmc_auth.Entities;

namespace dmc_auth.Hydra.Models
{
  public class ConsentAcceptAccessTokenBody
  {
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    public ConsentAcceptAccessTokenBody(string[] roles, ApplicationUser user)
    {
      Roles = roles;
      Name = user.UserName;
    }
  }
  public class ConsentAcceptIDTokenBody
  {
    [JsonPropertyName("roles")]
    public string[] Roles { get; set; }
    [JsonPropertyName("unique_name")]
    public string UniqueName { get; set; }
    public ConsentAcceptIDTokenBody(string[] roles, ApplicationUser user) : base()
    {
      Roles = roles;
      UniqueName = user.UserName;
    }
  }
  public class ConsentAcceptSessionBody
  {
    [JsonPropertyName("access_token")]
    public ConsentAcceptAccessTokenBody AccessToken { get; set; }
    [JsonPropertyName("id_token")]
    public ConsentAcceptIDTokenBody IdToken { get; set; }
    public ConsentAcceptSessionBody(string[] roles, ApplicationUser user)
    {
      IdToken = new ConsentAcceptIDTokenBody(roles, user);
      AccessToken = new ConsentAcceptAccessTokenBody(roles, user);
    }
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
      GrantScope = consent.RequestedScope;
      GrantAccessTokenAudience = consent.RequestedAccessTokenAudience;
      Session = new ConsentAcceptSessionBody(roles, user);
    }
  }

  public class AcceptConsentResponse
  {
    [JsonPropertyName("redirect_to")]
    public string RedirectTo { get; set; }
  }
}
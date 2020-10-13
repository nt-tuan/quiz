using dmc_auth.Entities;

namespace dmc_auth.Hydra.Models
{
  public class ConsentAcceptAccessTokenBody
  {
    public string[] roles { get; set; }
    public string name { get; set; }
    public ConsentAcceptAccessTokenBody(string[] roles, ApplicationUser user)
    {
      this.roles = roles;
      this.name = user.UserName;
    }
  }
  public class ConsentAcceptIDTokenBody
  {
    public string[] roles { get; set; }
    public string unique_name { get; set; }
    public ConsentAcceptIDTokenBody(string[] roles, ApplicationUser user) : base()
    {
      this.roles = roles;
      this.unique_name = user.UserName;
    }
  }
  public class ConsentAcceptSessionBody
  {
    public ConsentAcceptAccessTokenBody access_token { get; set; }
    public ConsentAcceptIDTokenBody id_token { get; set; }
    public ConsentAcceptSessionBody(string[] roles, ApplicationUser user)
    {
      id_token = new ConsentAcceptIDTokenBody(roles, user);
      access_token = new ConsentAcceptAccessTokenBody(roles, user);
    }
  }
  public class AcceptConsentRequest
  {
    public string[] grant_scope { get; set; }
    public string[] grant_access_token_audience { get; set; }
    public bool remember { get; set; } = true;
    public ulong remember_for { get; set; } = Constant.GetRememberDuration();
    public ConsentAcceptSessionBody session { get; set; }
    public AcceptConsentRequest(ConsentInfo consent, string[] roles, ApplicationUser user)
    {
      grant_scope = consent.requested_scope;
      grant_access_token_audience = consent.requested_access_token_audience;
      session = new ConsentAcceptSessionBody(roles, user);
    }
  }

  public class AcceptConsentResponse
  {
    public string redirect_to { get; set; }
  }
}
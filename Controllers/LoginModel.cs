using System;
using System.Collections.Generic;
namespace dmc_auth.Controllers
{
  public class LoginModel
  {
    public string login_challenge { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string acr { get; set; }
    public string force_subject_identifier { get; set; }
    public string remember { get; set; }
    public string remember_for { get; set; }
    public string subject { get; set; }

    public Dictionary<string, string> GetAcceptBody()
    {
      return new Dictionary<string, string>
      {
        {"acr", username},
        {"force_subject_identifier", force_subject_identifier},
        {"remember",remember},
        {"remember_for",remember_for},
        {"subject", subject}
      };
    }
  }

  public class AcceptLoginRequest
  {
    public string acr { get; set; }
    public string force_subject_identifier { get; set; }
    public bool remember { get; set; }
    public Int64 remember_for { get; set; }
    public string subject { get; set; }

    public AcceptLoginRequest(string userId)
    {
      subject = userId;
      acr = "user-password";
      remember = true;
      remember_for = (Int64)Constant.GetRememberDuration();
    }
  }

  public class AcceptLoginResponse
  {
    public string redirect_to { get; set; }
  }

  public class AcceptConsentResponse
  {
    public string redirect_to { get; set; }
  }
}

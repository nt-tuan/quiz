using System;

namespace dmc_auth.Hydra.Models
{
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
}
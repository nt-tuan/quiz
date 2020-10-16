using System;
using System.Text.Json.Serialization;

namespace dmc_auth.Hydra.Models
{
  public class AcceptLoginRequest
  {
    public string Acr { get; set; }
    [JsonPropertyName("force_subject_identifier")]
    public string ForceSubjectIdentifier { get; set; }
    public bool Remember { get; set; }
    [JsonPropertyName("remember_for")]
    public long RememberFor { get; set; }
    public string Subject { get; set; }

    public AcceptLoginRequest(string userId)
    {
      Subject = userId;
      Acr = "user-password";
      Remember = true;
      RememberFor = (long)Constant.GetRememberDuration();
    }
  }

  public class AcceptLoginResponse
  {
    [JsonPropertyName("redirect_to")]
    public string RedirectTo { get; set; }
  }
}
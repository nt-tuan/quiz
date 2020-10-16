using System.Text.Json.Serialization;

namespace dmc_auth.Controllers.Models
{
  public class Login
  {
    public string Username { get; set; }
    public string Password { get; set; }
    [JsonPropertyName("login_challenge")]
    public string LoginChallenge { get; set; }
  }
}
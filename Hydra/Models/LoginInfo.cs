using System.Text.Json.Serialization;

namespace dmc_auth.Hydra.Models
{
  public class LoginInfo
  {
    [JsonPropertyName("subject")]
    public string Subject { get; set; }
    [JsonPropertyName("skip")]
    public bool Skip { get; set; }
    [JsonPropertyName("username")]
    public string Username { get; set; }
  }
}
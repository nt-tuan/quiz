using System.Text.Json.Serialization;

namespace dmc_auth.Hydra.Models
{
  public class ConsentInfo
  {
    [JsonPropertyName("requested_access_token_audience")]
    public string[] RequestedAccessTokenAudience { get; set; }
    [JsonPropertyName("requested_scope")]
    public string[] RequestedScope { get; set; }
    public string Subject { get; set; }

  }
}
using System.Text.Json.Serialization;

namespace dmc_auth.Hydra.Models
{  
  public class AcceptLogoutResponse
  {
    [JsonPropertyName("redirect_to")]
    public string RedirectTo { get; set; }
  }
}
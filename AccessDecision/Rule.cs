using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace dmc_auth.AccessDecision
{
  public class Rule
  {
    [JsonPropertyName("path")]
    public string Path { get; set; }
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("methods")]
    public List<string> Methods { get; set; }
  }
}
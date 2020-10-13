namespace dmc_auth.Hydra.Models
{
  public class ConsentInfo
  {
    public string[] requested_access_token_audience { get; set; }
    public string[] requested_scope { get; set; }
    public string subject { get; set; }

  }
}
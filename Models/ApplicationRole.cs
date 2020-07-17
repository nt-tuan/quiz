using Microsoft.AspNetCore.Identity;
namespace dmc_auth.Models
{
  public class ApplicationRole : IdentityRole
  {
    public string Description { get; set; }
  }
}
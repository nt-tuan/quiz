using Microsoft.AspNetCore.Identity;
namespace dmc_auth.Entities
{
  public class ApplicationRole : IdentityRole
  {
    public string Description { get; set; }
  }
}
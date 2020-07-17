using Microsoft.AspNetCore.Identity;

namespace dmc_auth.Models
{
  public class ApplicationUser : IdentityUser<string>
  {
    public Employee Employee { get; set; }
  }
}

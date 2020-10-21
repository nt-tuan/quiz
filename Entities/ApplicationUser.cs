using Microsoft.AspNetCore.Identity;

namespace dmc_auth.Entities
{
  public class ApplicationUser : IdentityUser<string>
  {
    public Employee Employee { get; set; }
    public string Image { get; set; }
    public string Fullname { get; set; }
    public string Nickname { get; set; }
  }
}

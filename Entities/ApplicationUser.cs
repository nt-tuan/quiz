using Microsoft.AspNetCore.Identity;

namespace dmc_auth.Entities
{
    public class ApplicationUser : IdentityUser<string>
    {
        public Employee Employee { get; set; }
    }
}

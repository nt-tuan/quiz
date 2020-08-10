using System.Collections.Generic;

namespace dmc_auth.Controllers.Models
{
    public class CreateUserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
    }
}
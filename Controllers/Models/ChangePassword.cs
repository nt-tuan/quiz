using System.Collections.Generic;
using dmc_auth.Entities;

namespace dmc_auth.Controllers.Models
{
  public class ChangePasswordModel
  {
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
  }
}
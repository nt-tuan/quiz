using System.Collections.Generic;
using dmc_auth.Entities;

namespace dmc_auth.Controllers.Models
{
  public class ChangePasswordModel
  {
    public string newPassword { get; set; }
    public string oldPassword { get; set; }
  }
}
using System;
using Microsoft.EntityFrameworkCore;
namespace dmc_auth.Models
{
  [Owned]
  public class Person
  {
    public enum PersonGender { MALE = 0, FEMALE = 1 }
    public string IdentityNumber { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime? Birthday { get; set; }
    public int Gender { get; set; }
  }
}
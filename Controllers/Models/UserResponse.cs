using System;
using System.Collections.Generic;
using dmc_auth.Entities;

namespace dmc_auth.Controllers.Models
{
  public class DepartmentResponse
  {
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Shortname { get; set; }
    public string Code { get; set; }

    public DepartmentResponse(Department entity)
    {
      Id = entity.Id;
      Fullname = entity.FullName;
      Shortname = entity.ShortName;
      Code = entity.Code;
    }
  }
  public class EmployeeResponse
  {
    public int Id { get; set; }
    public string Fullname { get; set; }
    public string Code { get; set; }
    public DateTime? Birthday { get; set; }
    public int Gender { get; set; }
    public DepartmentResponse Department { get; set; }
    public EmployeeResponse(Employee entity)
    {
      Id = entity.Id;
      Fullname = entity.FullName;
      Code = entity.Code;
      Birthday = entity.Birthday;
      Gender = entity.Gender;
      if (entity.Department != null)
      {
        Department = new DepartmentResponse(entity.Department);
      }
    }
  }
  public class UserResponse
  {
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public IList<string> Roles { get; set; }
    public bool LockoutEnable { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public EmployeeResponse Employee { get; set; }
    public UserResponse(ApplicationUser entity, IList<string> roles)
    {
      Id = entity.Id;
      Username = entity.UserName;
      LockoutEnable = entity.LockoutEnabled;
      LockoutEnd = entity.LockoutEnd;
      Email = entity.Email;
      if (entity.Employee != null)
        Employee = new EmployeeResponse(entity.Employee);
      this.Roles = roles;
    }
  }
}
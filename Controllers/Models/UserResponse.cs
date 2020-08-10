using System;
using System.Collections.Generic;
using dmc_auth.Entities;

namespace dmc_auth.Controllers.Models
{
    public class DepartmentResponse
    {
        public string fullname { get; set; }
        public string shortname { get; set; }
        public string code { get; set; }

        public DepartmentResponse(Department entity)
        {
            fullname = entity.FullName;
            shortname = entity.ShortName;
            code = entity.Code;
        }
    }
    public class EmployeeResponse
    {
        public string fullname { get; set; }
        public string code { get; set; }
        public DateTime? birthday { get; set; }
        public int gender { get; set; }
        public DepartmentResponse department { get; set; }
        public EmployeeResponse(Employee entity)
        {
            fullname = entity.FullName;
            code = entity.Code;
            birthday = entity.Birthday;
            gender = entity.Gender;
            if (entity.Department != null)
            {
                department = new DepartmentResponse(entity.Department);
            }
        }
    }
    public class UserResponse
    {
        public string username { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
        public EmployeeResponse employee { get; set; }
        public UserResponse(ApplicationUser entity)
        {
            username = entity.UserName;
            email = entity.Email;
            if (entity.Employee != null)
                employee = new EmployeeResponse(entity.Employee);
        }

        public void SetRoles(List<string> roles)
        {
            this.roles = roles;
        }
    }
}
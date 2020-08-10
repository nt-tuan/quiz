using System;

namespace dmc_auth.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public string AppUserId { get; set; }
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

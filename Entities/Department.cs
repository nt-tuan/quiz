namespace dmc_auth.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }

        public string Location { get; set; }
        public string Location2 { get; set; }
        public string Location3 { get; set; }

        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }
        public int? ParentId { get; set; }
        public Department Parent { get; set; }

    }
}
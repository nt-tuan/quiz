namespace dmc_auth.Models
{
  public class Employee
  {
    public int Id { get; set; }
    public Person Person { get; set; }
    public int PersonId { get; set; }
    public string Code { get; set; }
    public int? DepartmentId { get; set; }
    public Department Department { get; set; }
    public string AppUserId { get; set; }
  }
}

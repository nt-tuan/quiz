using System.Collections;
using System.Collections.Generic;

namespace ThanhTuan.Quiz.Entities
{
  public class Exam : BaseEntity
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Question> Questions { get; set; }
  }
}
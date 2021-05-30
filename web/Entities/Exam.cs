using System.Collections;
using System.Collections.Generic;
namespace ThanhTuan.Quiz.Entities
{
  public class Exam : BaseEntity
  {
    public string Slug { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Question> Questions { get; set; }

    // Duration of exams
    public int? Duration { get; set; }
    public string Image { get; set; }
    public bool IsVisible { get; set; }

    public ICollection<Label> Labels { get; set; }
  }
}
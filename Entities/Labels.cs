using System.Collections;
using System.Collections.Generic;

namespace ThanhTuan.Quiz.Entities
{
  public class Label : BaseEntity
  {
    public string KeyId { get; set; }
    public LabelKey Key { get; set; }
    public string Value { get; set; }
    public string DisplayName { get; set; }
    public string IsVisible { get; set; }
    public string Image { get; set; }
    public ICollection<Exam> Exams { get; set; }
    public ICollection<Collection> Collections { get; set; }
  }
}
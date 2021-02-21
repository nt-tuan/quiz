using System.Collections;
using System.Collections.Generic;

namespace ThanhTuan.Quiz.Entities
{
  public class ExamLabel : BaseEntity
  {
    public int ExamId { get; set; }
    public Exam Exam { get; set; }
    public int LabelId { get; set; }
    public Label Label { get; set; }
  }
}
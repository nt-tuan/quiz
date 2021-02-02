using System.Collections;
using System.Collections.Generic;

namespace ThanhTuan.Quiz.Entities
{
  public class Question
  {
    public int Id { get; set; }
    public int ExamId { get; set; }
    public string Content { get; set; }

    public ICollection<AnswerOption> AnswerOptions { get; set; }
    public Exam Exam { get; set; }
  }
}
using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ThanhTuan.Quiz.Entities
{
  public class Question : BaseEntity
  {
    public int ExamId { get; set; }
    public string Content { get; set; }

    public ICollection<AnswerOption> AnswerOptions { get; set; }
    [JsonIgnore]
    public Exam Exam { get; set; }
  }
}
using System.Collections.Generic;

namespace ThanhTuan.Scripts.UpdateQuiz.Models
{
  public class Question
  {
    public string Id { get; set; }
    public string Content { get; set; }
    public List<Answer> Answers { get; set; }
  }
}
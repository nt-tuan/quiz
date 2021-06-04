using System.Collections.Generic;

namespace ThanhTuan.Scripts.UpdateQuiz.Models
{
  public class Answer
  {
    public string Id { get; set; }
    public string Value { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
  }
}
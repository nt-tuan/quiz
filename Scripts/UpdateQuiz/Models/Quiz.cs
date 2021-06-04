using System.Collections.Generic;

namespace ThanhTuan.Scripts.UpdateQuiz.Models
{
  public class Quiz
  {
    public int Id { get; set; }
    public Dictionary<string, string> Tag { get; set; }
    public string Title { get; set; }
    public string Subtitle { get; set; }
    public List<Question> Questions { get; set; }
  }
}
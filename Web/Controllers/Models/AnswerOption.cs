using System;
using ThanhTuan.Quiz;
namespace ThanhTuan.Quiz.Controllers.Models
{
  public class AnswerOption
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
    public AnswerOption() { }
    public AnswerOption(Entities.AnswerOption entity)
    {
      Id = entity.Id;
      Content = entity.Content;
      IsCorrect = entity.IsCorrect;
    }
  }
}
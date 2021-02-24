using System;
using System.Collections.Generic;
using System.Linq;
using ThanhTuan.Quiz;
namespace ThanhTuan.Quiz.Controllers.Models
{
  public class Question
  {
    public int Id { get; set; }
    public string Content { get; set; }
    public List<AnswerOption> AnswerOptions { get; set; }
    public Question() { }
    public Question(Entities.Question entity)
    {
      Id = entity.Id;
      Content = entity.Content;
      AnswerOptions = entity.AnswerOptions?.Select(ans => new AnswerOption(ans)).ToList();
    }
  }
}
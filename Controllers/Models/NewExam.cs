using ThanhTuan.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ThanhTuan.Quiz.Controllers.Models
{
  public class NewAnswerOption
  {
    public string Content { get; set; }
    public bool IsCorrect { get; set; }
  }
  public class NewQuestion
  {
    public string Content { get; set; }
    public List<AnswerOption> AnswerOptions { get; set; }
  }
  public class NewExam
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int> LabelIds { get; set; }
    public int? Duration { get; set; }
    public string Image { get; set; }
    public List<NewQuestion> Questions { get; set; }

    public Entities.Exam ToEntity()
    {
      return new Entities.Exam
      {
        Title = Title,
        Description = Description,
        Duration = Duration,
        Image = Image,
        Labels = LabelIds.Select(id => new Entities.Label
        {
          Id = id
        }).ToList(),
        Questions = Questions?.Select(question => new Entities.Question
        {
          Content = question.Content,
          AnswerOptions = question.AnswerOptions?.Select(ans => new Entities.AnswerOption
          {
            Content = ans.Content,
            IsCorrect = ans.IsCorrect
          }).ToList()
        }).ToList()
      };
    }
  }
}
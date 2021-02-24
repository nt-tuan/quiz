using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ThanhTuan.Quiz;
namespace ThanhTuan.Quiz.Controllers.Models
{
  public class ExamEntry
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<Label> Labels { get; set; }
    public int? Duration { get; set; }
    public string Image { get; set; }
    public ExamEntry() { }
    public ExamEntry(Entities.Exam entity)
    {
      Id = entity.Id;
      Title = entity.Title;
      Description = entity.Description;
      CreatedAt = entity.CreatedAt;
      UpdatedAt = entity.UpdatedAt;
      Labels = entity.Labels?.Select(label => new Label(label)).ToList();
      Duration = entity.Duration;
      Image = entity.Image;
    }
  }

  public class Exam : ExamEntry
  {
    public List<Question> Questions { get; set; }
    public Exam() { }
    public Exam(Entities.Exam entity) : base(entity)
    {
      Questions = entity.Questions?.Select(question => new Question(question)).ToList();
    }
    public Entities.Exam ToEntity()
    {
      return new Entities.Exam
      {
        Id = Id,
        Title = Title,
        Description = Description,
        Duration = Duration,
        Image = Image,
        Questions = Questions?.Select(question => new Entities.Question
        {
          Id = question.Id,
          Content = question.Content,
          AnswerOptions = question.AnswerOptions?.Select(ans => new Entities.AnswerOption
          {
            Id = ans.Id,
            Content = ans.Content,
            IsCorrect = ans.IsCorrect
          }).ToList()
        }).ToList()
      };
    }
  }
}
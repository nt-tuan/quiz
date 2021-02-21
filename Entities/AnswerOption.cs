using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ThanhTuan.Quiz.Entities
{
  public class AnswerOption : BaseEntity
  {
    public string Content { get; set; }
    public int QuestionId { get; set; }
    [JsonIgnore]
    public Question Question { get; set; }
    public bool IsCorrect { get; set; }
  }
}
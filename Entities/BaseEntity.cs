using System;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ThanhTuan.Quiz.Entities
{
  public class BaseEntity
  {
    public DateTimeOffset? CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
  }
}
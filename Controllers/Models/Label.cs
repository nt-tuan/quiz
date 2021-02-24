using System;
using ThanhTuan.Quiz;
namespace ThanhTuan.Quiz.Controllers.Models
{
  public class Label
  {
    public string KeyId { get; set; }
    public string Value { get; set; }
    public string DisplayName { get; set; }
    public Label() { }
    public Label(Entities.Label entity)
    {
      KeyId = entity.KeyId;
      Value = entity.Value;
      DisplayName = entity.DisplayName;
    }
    public Entities.Label ToEntity()
    {
      return new Entities.Label
      {
        KeyId = KeyId,
        Value = Value,
        DisplayName = DisplayName
      };
    }
  }
}
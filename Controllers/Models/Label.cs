using System;
using ThanhTuan.Quiz;
namespace ThanhTuan.Quiz.Controllers.Models
{
  public class Label
  {
    public int Id { get; set; }
    public string KeyId { get; set; }
    public string Value { get; set; }
    public string DisplayName { get; set; }
    public Label() { }
    public Label(Entities.Label entity)
    {
      Id = entity.Id;
      KeyId = entity.KeyId;
      Value = entity.Value;
      DisplayName = entity.DisplayName;
    }
    public Entities.Label ToEntity()
    {
      return new Entities.Label
      {
        Id = Id,
        KeyId = KeyId,
        Value = Value,
        DisplayName = DisplayName
      };
    }
  }
}
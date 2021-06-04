using System.Collections;
using System.Collections.Generic;

namespace ThanhTuan.Quiz.Entities
{
  public class Collection : BaseEntity
  {
    public string DisplayName { get; set; }
    public string Slug { get; set; }
    public int Rank { get; set; }
    public ICollection<Label> Labels { get; set; }
    public string Image { get; set; }
    public bool IsVisible { get; set; }
  }
}
using System.Collections.Generic;
using System.Linq;
using dmc_auth.Migrations;

namespace ThanhTuan.Quiz.Controllers.Models
{
  public class CollectionEntry
  {
    public int Id { get; set; }
    public string Slug { get; set; }
    public string DisplayName { get; set; }
    public string Image { get; set; }
    public bool IsVisible { get; set; }

    public CollectionEntry()
    {

    }
    public CollectionEntry(Entities.Collection entity)
    {
      Id = entity.Id;
      Slug = entity.Slug;
      DisplayName = entity.DisplayName;
      Image = entity.Image;
      IsVisible = entity.IsVisible;

    }
  }

  public class Collection : CollectionEntry
  {
    public List<Label> Labels { get; set; }
    public Collection() { }
    public Collection(Entities.Collection entity) : base(entity)
    {
      Labels = entity.Labels?.Select(label => new Label(label)).ToList();
    }
    public Entities.Collection ToNewEntity()
    {
      return new Entities.Collection
      {
        Slug = Slug,
        DisplayName = DisplayName,
        Image = Image,
      };
    }
  }
  public class EditCollection
  {
    public string Slug { get; set; }
    public string DisplayName { get; set; }
    public string Image { get; set; }
    public bool IsVisible { get; set; }
    public EditCollection()
    {
    }
    public Entities.Collection ToEntity()
    {
      return new Entities.Collection
      {
        Slug = Slug,
        DisplayName = DisplayName,
        Image = Image,
        IsVisible = IsVisible
      };
    }
  }
}
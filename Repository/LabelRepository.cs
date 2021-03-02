using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.DBContext;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Repositories
{
  public class LabelRepository
  {
    private readonly ApplicationDbContext _db;
    private readonly Repository _repo;
    public LabelRepository(ApplicationDbContext db)
    {
      _db = db;
      _repo = new Repository(db);
    }
    public async Task<List<Label>> GetLabels()
    {
      return await _repo.List(_db.Labels.AsQueryable());
    }
    public async Task<Label> AddLabel(Label label, string by)
    {
      return await _repo.Add(label, by);
    }

    public async Task<Label> UpdateLabel(Label label, string by)
    {
      return await _repo.Update(label, by);
    }

    public async Task DeleteLabel(int id, string by)
    {
      await _repo.Delete<Label>(id, by);
    }

    public async Task<List<Collection>> GetLabelCollections()
    {
      return await _repo.List(_db.Colletions.OrderBy(u => u.Rank));
    }

    public async Task<Collection> AddCollection(Collection entity, string by)
    {
      return await _repo.Add(entity, by);
    }

    public async Task<Collection> UpdateCollection(Collection entity, string by)
    {
      return await _repo.Update(entity, by);
    }

    public async Task DeleteCollection(int id, string by)
    {
      await _repo.Delete<Collection>(id, by);
    }

    public async Task RemoveLabelsFromCollection(int collectionId, int labelId)
    {
      var labels = await _db.Set<LabelCollection>().Where(u => u.CollectionId == collectionId && u.LabelId == labelId).ToListAsync();
      _db.RemoveRange(labels);
      await _db.SaveChangesAsync();
    }

    public async Task AddLabelsToCollection(int collectionId, int labelId)
    {
      var labelCollection = new LabelCollection
      {
        CollectionId = collectionId,
        LabelId = labelId,
      };
      _db.Add(labelCollection);
      await _db.SaveChangesAsync();
    }
  }
}
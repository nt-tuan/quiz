using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.DBContext;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Repositories
{
  public class Repository
  {
    private readonly ApplicationDbContext _db;
    public Repository(ApplicationDbContext db)
    {
      _db = db;
    }

    #region Private functions    
    public async Task<T> FirstOrDefault<T>(IQueryable<T> query) where T : BaseEntity
    {
      return await query.Where(u => u.DeletedAt == null).FirstOrDefaultAsync();
    }
    public async Task<T> Find<T>(int id) where T : BaseEntity
    {
      return await _db.Set<T>().FirstOrDefaultAsync(u => u.DeletedAt == null && u.Id == id);
    }
    public async Task<List<T>> List<T>(IQueryable<T> query) where T : BaseEntity
    {
      return await query.Where(u => u.DeletedAt == null).ToListAsync();
    }

    public async Task<List<T>> List<T>() where T : BaseEntity
    {
      return await _db.Set<T>().Where(u => u.DeletedAt == null).ToListAsync();
    }
    public async Task<T> Update<T>(T entity, string by) where T : BaseEntity
    {
      entity.UpdatedBy = by;
      entity.UpdatedAt = DateTimeOffset.Now;
      _db.Update(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task<T> Add<T>(T entity, string by) where T : BaseEntity
    {
      entity.CreatedAt = DateTimeOffset.Now;
      entity.CreatedBy = by;
      _db.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    public async Task Delete<T>(int id, string by) where T : BaseEntity
    {
      var entity = await Find<T>(id);
      if (entity != null)
      {
        entity.DeletedBy = by;
        entity.DeletedAt = DateTimeOffset.Now;
        await _db.SaveChangesAsync();
      }
    }

    public async Task Delete<T>(T entity, string by) where T : BaseEntity
    {
      entity.DeletedAt = DateTimeOffset.Now;
      entity.DeletedBy = by;
      _db.Update(entity);
      await _db.SaveChangesAsync();
    }
    #endregion
  }
}
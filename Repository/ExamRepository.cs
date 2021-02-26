using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.DBContext;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Repositories
{
  public class ExamRepository
  {
    private readonly ApplicationDbContext _db;
    public ExamRepository(ApplicationDbContext db)
    {
      _db = db;
    }

    #region Private functions    
    async Task<T> FirstOrDefault<T>(IQueryable<T> query) where T : BaseEntity
    {
      return await query.Where(u => u.DeletedAt == null).FirstOrDefaultAsync();
    }
    async Task<T> Find<T>(IQueryable<T> query, int id) where T : BaseEntity
    {
      return await query.FirstOrDefaultAsync(u => u.DeletedAt == null && u.Id == id);
    }
    async Task<List<T>> List<T>(IQueryable<T> query) where T : BaseEntity
    {
      return await query.Where(u => u.DeletedAt == null).ToListAsync();
    }
    async Task Update<T>(T entity, string by) where T : BaseEntity
    {
      entity.UpdatedBy = by;
      entity.UpdatedAt = DateTimeOffset.Now;
      _db.Update(entity);
      await _db.SaveChangesAsync();
    }

    async Task<T> Add<T>(T entity, string by) where T : BaseEntity
    {
      entity.CreatedAt = DateTimeOffset.Now;
      entity.CreatedBy = by;
      _db.Add(entity);
      await _db.SaveChangesAsync();
      return entity;
    }

    async Task Delete<T>(T entity, string by) where T : BaseEntity
    {
      entity.DeletedAt = DateTimeOffset.Now;
      entity.DeletedBy = by;
      _db.Update(entity);
      await _db.SaveChangesAsync();
    }
    #endregion

    public async Task<List<Exam>> GetExams(string label)
    {
      return await List(_db.Exams.
      Include(exam => exam.Labels).
      Where(u => u.Labels.Where(u => u.Value == label).Any()));
    }

    public async Task<Exam> GetExamBySlug(string slug)
    {
      return await FirstOrDefault(_db.Exams.Include(u => u.Questions).ThenInclude(u => u.AnswerOptions).Where(u => u.Slug == slug));
    }

    public async Task<Exam> UpdateExam(Exam exam, string by)
    {
      await Update(exam, by);
      return exam;
    }

    public async Task<Exam> AddExam(Exam newExam, string by)
    {
      return await Add(newExam, by);
    }

    public async Task DeleteExam(int examId, string by)
    {
      var exam = await Find(_db.Exams, examId);
      await Delete(exam, by);
    }

    public async Task AttachLabel(int examId, int labelId)
    {
      _db.Add(new ExamLabel { ExamId = examId, LabelId = labelId });
      await _db.SaveChangesAsync();
    }

    public async Task DetachLabel(int examId, int labelId)
    {
      var entity = await _db.Set<ExamLabel>().FirstOrDefaultAsync(u => u.ExamId == examId && u.LabelId == labelId);
      _db.Remove(entity);
      await _db.SaveChangesAsync();
    }

    public async Task<List<Label>> GetLabels()
    {
      return await List(_db.Labels.AsQueryable());
    }

    public async Task<Label> AddLabel(Label label, string by)
    {
      var entity = await Add(label, by);
      return entity;
    }

    public async Task<Label> UpdateLabel(Label label, string by)
    {
      await Update(label, by);
      return label;
    }

    public async Task DeleteLabel(int id, string by)
    {
      var label = await Find(_db.Labels.AsQueryable(), id);
      await Delete(label, by);
    }
  }
}
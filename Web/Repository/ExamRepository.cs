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
    private readonly Repository _repo;
    public ExamRepository(ApplicationDbContext db)
    {
      _db = db;
      _repo = new Repository(db);
    }

    public async Task<List<Exam>> GetAllExams()
    {
      return await _repo.List(_db.Exams.
      Include(exam => exam.Labels));
    }

    public async Task<List<Exam>> GetExams(string label)
    {
      return await _repo.List(_db.Exams.
      Include(exam => exam.Labels).
      Where(u => u.Labels.Where(u => u.Value == label).Any()));
    }

    public async Task<Exam> GetExamBySlug(string slug)
    {
      return await _repo.FirstOrDefault(_db.Exams
      .Include(u => u.Questions)
      .ThenInclude(u => u.AnswerOptions)
      .Include(u => u.Labels)
      .Where(u => u.Slug == slug));
    }

    public async Task<Exam> UpdateExam(Exam exam, string by)
    {
      await _repo.Update(exam, by);
      return exam;
    }

    public async Task<Exam> AddExam(Exam newExam, string by)
    {
      return await _repo.Add(newExam, by);
    }

    public async Task DeleteExam(int examId, string by)
    {
      await _repo.Delete<Exam>(examId, by);
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
  }
}
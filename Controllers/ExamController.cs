using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThanhTuan.Quiz.Data;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("api")]
  [ApiController]
  public class ExamController : ControllerBase
  {
    private readonly ApplicationDbContext _db;

    public ExamController(ApplicationDbContext db)
    {
      _db = db;
    }

    /// <summary>
    /// Create a new exam
    /// </summary>
    /// <param name="exam"></param>
    /// <returns>exam</returns>
    [HttpPost]
    public async Task<ActionResult<Exam>> CreateExam(Exam exam)
    {
      exam.CreatedBy = Request.Headers["X-User"];
      exam.CreatedAt = DateTimeOffset.Now;
      _db.Add(exam);
      await _db.SaveChangesAsync();
      return exam;
    }

    /// <summary>
    /// Get an exam by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>exam</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Exam>> GetExam(int id)
    {
      return await _db.Exams.Include(u => u.Questions).ThenInclude(u => u.AnswerOptions).FirstAsync(u => u.Id == id);
    }

    /// <summary>
    /// Get a list of exams
    /// </summary>
    /// <returns>list of exams</returns>
    [HttpGet]
    public async Task<ActionResult<List<Exam>>> GetExamList()
    {
      return await _db.Exams.ToListAsync();
    }
    /// <summary>
    /// Update an exam
    /// </summary>
    /// <param name="exam"></param>
    /// <returns>exam</returns>
    [HttpPut]
    public async Task<ActionResult<Exam>> UpdateExam(Exam exam)
    {
      _db.Update(exam);
      await _db.SaveChangesAsync();
      return exam;
    }

    /// <summary>
    /// Delete an exam by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteExam(int id)
    {
      var exam = await _db.Exams.FirstAsync(u => u.Id == id);
      exam.DeletedAt = DateTimeOffset.Now;
      exam.DeletedBy = Request.Headers["X-User"];
      _db.Update(exam);
      await _db.SaveChangesAsync();
      return Ok();
    }
  }
}
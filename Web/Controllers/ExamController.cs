using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThanhTuan.Quiz.Controllers.Models;
using ThanhTuan.Quiz.Repositories;
using ThanhTuan.Quiz.Services;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("api/exam")]
  [ApiController]
  public class ExamController : ControllerBase
  {
    private readonly ExamRepository _repo;
    private readonly Authorizer _authorizer;
    public ExamController(ExamRepository repo, Authorizer authorizer)
    {
      _repo = repo;
      _authorizer = authorizer;
    }

    /// <summary>
    /// Create a new exam
    /// </summary>
    /// <param name="exam"></param>
    /// <returns>exam</returns>
    [HttpPost]
    public async Task<ActionResult<Exam>> CreateExam(Exam exam)
    {
      var entity = exam.ToEntity();
      await _repo.AddExam(entity, _authorizer.GetUser());
      return new Exam(entity);
    }

    /// <summary>
    /// Get an exam by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <returns>exam</returns>
    [HttpGet]
    public async Task<ActionResult<Exam>> GetExam(string slug)
    {
      var entity = await _repo.GetExamBySlug(slug);
      if (entity == null) return NotFound();
      return new Exam(entity);
    }

    /// <summary>
    /// Get a list of exams
    /// </summary>
    /// <param name="label"></param>
    /// <returns>list of exams</returns>
    [HttpGet("list")]
    public async Task<ActionResult<List<Exam>>> GetExamList(string label)
    {
      var exams = await _repo.GetExams(label);
      return exams.Select(exam => new Exam(exam)).ToList();
    }

    /// <summary>
    /// Get all exams
    /// </summary>    
    /// <returns>list of exams</returns>
    [HttpGet("all")]
    public async Task<ActionResult<List<Exam>>> GetAllExams()
    {
      var exams = await _repo.GetAllExams();
      return exams.Select(exams => new Exam()).ToList();
    }

    /// <summary>
    /// Update an exam
    /// </summary>
    /// <param name="model"></param>
    /// <returns>exam</returns>
    [HttpPut]
    public async Task<ActionResult<ExamEntry>> UpdateExam(ExamEntry model)
    {
      var entity = model.ToEntity();
      await _repo.UpdateExam(entity, _authorizer.GetUser());
      return model;
    }

    /// <summary>
    /// Delete an exam by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("id/{id}")]
    public async Task<ActionResult> DeleteExam(int id)
    {
      await _repo.DeleteExam(id, _authorizer.GetUser());
      return Ok();
    }

    /// <summary>
    /// Add a label to an exam
    /// </summary>
    /// <param name="id"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpPut("id/{id}/label/{label}")]
    public async Task<ActionResult> AddExamLabel(int id, int label)
    {
      await _repo.AttachLabel(id, label);
      return Ok();
    }

    /// <summary>
    /// Remove a label of an exam
    /// </summary>
    /// <param name="id"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpDelete("id/{id}/label/{label}")]
    public async Task<ActionResult> DeleteExamLabel(int id, int label)
    {
      await _repo.DetachLabel(id, label);
      return Ok();
    }
  }
}
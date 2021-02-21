using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThanhTuan.Quiz.Entities;
using ThanhTuan.Quiz.Repositories;
using ThanhTuan.Quiz.Services;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("api")]
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
    [HttpPost("exam")]
    public async Task<ActionResult<Exam>> CreateExam(Exam exam)
    {
      return await _repo.AddExam(exam, exam.CreatedBy = _authorizer.GetUser());
    }

    /// <summary>
    /// Get an exam by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <returns>exam</returns>
    [HttpGet("exam")]
    public async Task<ActionResult<Exam>> GetExam(string slug)
    {
      return await _repo.GetExamBySlug(slug);
    }

    /// <summary>
    /// Get a list of exams
    /// </summary>
    /// <param name="label"></param>
    /// <returns>list of exams</returns>
    [HttpGet("exams")]
    public async Task<ActionResult<List<Exam>>> GetExamList(int label)
    {
      return await _repo.GetExams(label);
    }
    /// <summary>
    /// Update an exam
    /// </summary>
    /// <param name="exam"></param>
    /// <returns>exam</returns>
    [HttpPut("exam")]
    public async Task<ActionResult<Exam>> UpdateExam(Exam exam)
    {
      await _repo.UpdateExam(exam, _authorizer.GetUser());
      return exam;
    }

    /// <summary>
    /// Delete an exam by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("exam/{id}")]
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
    [HttpPut("exam/{id}/label/{label}")]
    public async Task<ActionResult> AddExamLabel(int id, int label)
    {
      await _repo.AttachLabel(id, label);
      return Ok();
    }

    /// <summary>
    /// Remote a label of an exam
    /// </summary>
    /// <param name="id"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpDelete("exam/{id}/label/{label}")]
    public async Task<ActionResult> DeleteExamLabel(int id, int label)
    {
      await _repo.DetachLabel(id, label);
      return Ok();
    }

    /// <summary>
    /// Create a new label
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpPost("label")]
    public async Task<ActionResult<Label>> AddLabel(Label label)
    {
      return await _repo.AddLabel(label, _authorizer.GetUser());
    }

    /// <summary>
    /// Update a label
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpPut("label")]
    public async Task<ActionResult<Label>> UpdateLabel(Label label)
    {
      return await _repo.UpdateLabel(label, _authorizer.GetUser());
    }

    /// <summary>
    /// Delete a label
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("label/{id}")]
    public async Task<ActionResult> DeleteLabel(int id)
    {
      await _repo.DeleteLabel(id, _authorizer.GetUser());
      return Ok();
    }
  }
}
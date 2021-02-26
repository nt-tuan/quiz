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
      var entity = exam.ToEntity();
      await _repo.AddExam(entity, _authorizer.GetUser());
      return new Exam(entity);
    }

    /// <summary>
    /// Get an exam by slug
    /// </summary>
    /// <param name="slug"></param>
    /// <returns>exam</returns>
    [HttpGet("exam")]
    public async Task<ActionResult<Exam>> GetExam(string slug)
    {
      var entity = await _repo.GetExamBySlug(slug);
      return new Exam(entity);
    }

    /// <summary>
    /// Get a list of exams
    /// </summary>
    /// <param name="label"></param>
    /// <returns>list of exams</returns>
    [HttpGet("exams")]
    public async Task<ActionResult<List<Exam>>> GetExamList(string label)
    {
      var exams = await _repo.GetExams(label);
      return exams.Select(exam => new Exam(exam)).ToList();
    }
    /// <summary>
    /// Update an exam
    /// </summary>
    /// <param name="exam"></param>
    /// <returns>exam</returns>
    [HttpPut("exam")]
    public async Task<ActionResult<Exam>> UpdateExam(Exam exam)
    {
      var entity = exam.ToEntity();
      await _repo.UpdateExam(entity, _authorizer.GetUser());
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
    /// Remove a label of an exam
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
    /// Get all labels
    /// </summary>
    /// <returns></returns>
    [HttpGet("labels")]
    public async Task<ActionResult<List<Label>>> GetLabels()
    {
      var entities = await _repo.GetLabels();
      return entities.Select(label => new Label(label)).ToList();
    }

    /// <summary>
    /// Create a new label
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpPost("label")]
    public async Task<ActionResult<Label>> AddLabel(Label label)
    {
      var newLabel = await _repo.AddLabel(label.ToEntity(), _authorizer.GetUser());
      return new Label(newLabel);
    }

    /// <summary>
    /// Update a label
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    [HttpPut("label")]
    public async Task<ActionResult<Label>> UpdateLabel(Label label)
    {
      var entity = label.ToEntity();
      entity = await _repo.UpdateLabel(entity, _authorizer.GetUser());
      return new Label(entity);
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
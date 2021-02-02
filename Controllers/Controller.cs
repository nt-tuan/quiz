using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ThanhTuan.Quiz.Data;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Controllers
{
  [Route("api")]
  [ApiController]
  public class Controller : ControllerBase
  {
    private readonly ApplicationDbContext _db;

    public Controller(ApplicationDbContext db)
    {
      _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Exam>> CreateExam(Exam exam)
    {
      _db.Add(exam);
      await _db.SaveChangesAsync();
      return exam;
    }

    [HttpGet]
    public async Task<ActionResult<Exam>> GetExam(int id)
    {
      return await _db.Exams.FindAsync(id);
    }

    [HttpPut]
    public async Task<ActionResult<Exam>> UpdateExam(Exam exam)
    {
      _db.Update(exam);
      await _db.SaveChangesAsync();
      return exam;
    }
  }
}
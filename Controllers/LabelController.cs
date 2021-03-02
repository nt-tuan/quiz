using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThanhTuan.Quiz.Repositories;
using ThanhTuan.Quiz.Controllers.Models;
using System.Linq;
using ThanhTuan.Quiz.Services;

namespace ThanhTuan.Quiz.Controllers
{
  [ApiController]
  [Route("api/label")]
  public class LabelController : ControllerBase
  {
    private readonly LabelRepository _repo;
    private readonly Authorizer _authorizer;
    public LabelController(LabelRepository repo, Authorizer authorizer)
    {
      _repo = repo;
      _authorizer = authorizer;
    }

    /// <summary>
    /// Get all labels
    /// </summary>
    /// <returns></returns>
    [HttpGet]
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
    [HttpPost]
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
    [HttpPut]
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
    [HttpDelete]
    public async Task<ActionResult> DeleteLabel(int id)
    {
      await _repo.DeleteLabel(id, _authorizer.GetUser());
      return Ok();
    }
  }
}
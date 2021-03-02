using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThanhTuan.Quiz.Repositories;
using ThanhTuan.Quiz.Controllers.Models;
using System.Linq;
using ThanhTuan.Quiz.Services;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Controllers
{
  [ApiController]
  [Route("api/collection")]
  public class CollectionController : ControllerBase
  {
    private readonly LabelRepository _repo;
    private readonly Authorizer _authorizer;
    public CollectionController(LabelRepository repo, Authorizer authorizer)
    {
      _repo = repo;
      _authorizer = authorizer;
    }

    [HttpGet]
    public async Task<List<Models.Collection>> GetCollections()
    {
      var entities = await _repo.GetLabelCollections();
      return entities.Select(entity => new Models.Collection(entity)).ToList();
    }

    [HttpPost()]
    public async Task<Models.Collection> AddCollection(EditCollection model)
    {
      var entity = model.ToEntity();
      await _repo.AddCollection(entity, _authorizer.GetUser());
      return new Models.Collection(entity);
    }

    [HttpPut("{id}")]
    public async Task<Models.Collection> UpdateCollection(int id, EditCollection model)
    {
      var entity = model.ToEntity();
      entity.Id = id;
      entity.Labels = null;
      var updatedEntity = await _repo.UpdateCollection(entity, _authorizer.GetUser());
      return new Models.Collection(updatedEntity);
    }

    [HttpPut("{id}/label/{labelId}")]
    public async Task<ActionResult> AddLabelToCollection(int id, int labelId)
    {
      await _repo.AddLabelsToCollection(id, labelId);
      return Ok();
    }

    [HttpDelete("{id}/label/{labelId}")]
    public async Task<ActionResult> RemoveLabelFromCollection(int id, int labelId)
    {
      await _repo.RemoveLabelsFromCollection(id, labelId);
      return Ok();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCollection(int id)
    {
      await _repo.DeleteCollection(id, _authorizer.GetUser());
      return Ok();
    }
  }
}
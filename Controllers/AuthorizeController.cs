using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dmc_auth.AccessDecision;
using dmc_auth.Hydra;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Web.Api
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthorizeController : ControllerBase
  {
    private readonly AccessDecision _decision;
    private readonly ILogger<AuthorizeController> _logger;
    private readonly IHydra _hydra;
    public AuthorizeController(AccessDecision decision, ILogger<AuthorizeController> logger, IHydra hydra)
    {
      _decision = decision;
      _logger = logger;
      _hydra = hydra;
    }
    [HttpGet]
    public async Task<ActionResult> Get()
    {
      var bearertoken = Request.Headers["Authorization"].ToString();
      var path = Request.Headers["X-Request-Uri"];
      var method = Request.Headers["X-Request-Method"];
      foreach (var pair in Request.Headers)
      {
        _logger.LogInformation("REQUEST HEADER ---- {0}:{1} ", pair.Key, pair.Value);
      }
      _logger.LogInformation("Auth Headers: ", Request.Headers);
      var rule = _decision.GetFirstMatchedRule(path, method);
      if (rule == null) return Ok();
      if (!string.IsNullOrEmpty(bearertoken)) return Unauthorized();
      if (bearertoken.Length <= "Bearer".Length) return Unauthorized();
      var accessToken = bearertoken["Bearer ".Length..];

      var result = await _hydra.InstropectToken(accessToken, null);
      if (!result.Active)
        return Unauthorized();
      Response.Headers.Add("X-Subject", result.Sub);
      Response.Headers.Add("X-User", result.Ext.Name);
      Response.Headers.Add("X-Roles", result.Ext.Roles);
      foreach (var pair in Response.Headers)
      {
        _logger.LogInformation("RESPONSE HEADER ---- {0}:{1} ", pair.Key, pair.Value);
      }
      return Ok();
    }
  }
}
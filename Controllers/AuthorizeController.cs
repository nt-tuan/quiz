using System.Threading.Tasks;
using dmc_auth.AccessDecision;
using dmc_auth.Hydra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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
      var rule = _decision.GetFirstMatchedRule(path, method);
      if (rule == null) return Ok();
      if (string.IsNullOrEmpty(bearertoken)) return Unauthorized();
      if (bearertoken.Length <= "Bearer".Length) return Unauthorized();
      var accessToken = bearertoken["Bearer ".Length..];
      try
      {
        var result = await _hydra.InstropectToken(accessToken, null);
        if (!result.Active)
          return Unauthorized();
        if (!string.IsNullOrEmpty(rule.Role) && !result.Ext.Roles.Contains(rule.Role))
          return Unauthorized();
        Response.Headers.Add("X-Subject", result.Sub);
        Response.Headers.Add("X-User", result.Ext.Name);
        Response.Headers.Add("X-Roles", string.Join(",", result.Ext.Roles));
        Response.Headers.Add("X-Scope", result.Scope);
        return Ok();
      }
      catch (Exception e)
      {
        _logger.LogInformation(e.Message);
        _logger.LogInformation(e.StackTrace);
        return Unauthorized(e.Message);
      }
    }
  }
}
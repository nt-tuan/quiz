using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using dmc_auth.AccessDecision;
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
        public AuthorizeController(AccessDecision decision, ILogger<AuthorizeController> logger)
        {
            _decision = decision;
            _logger = logger;
        }
        [HttpGet]
        public ActionResult Get()
        {
            var roles = Request.Headers["X-Roles"];
            var path = Request.Headers["X-Request-Uri"];
            var method = Request.Headers["X-Request-Method"];
            _logger.LogInformation("Path: {0} \nRoles: {1} \nMethod: {2}", path, roles, method);
            if (_decision.CanAccess(path, roles.ToString().Split(","), method))
            {
                return Ok();
            }
            return Unauthorized();
        }
    }
}
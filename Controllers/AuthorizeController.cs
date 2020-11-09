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
        public AuthorizeController(AccessDecision decision)
        {
            _decision = decision;
        }
        [HttpGet]
        public ActionResult Get()
        {
            var roles = Request.Headers["X-Roles"];
            var path = Request.Headers["X-Request-Uri"];
            if (_decision.CanAccess(path, roles.ToString().Split(",")))
            {
                return Ok();
            }
            return Unauthorized();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dmc_auth;
using dmc_auth.Controllers.Models;
using dmc_auth.Data;
using dmc_auth.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Web.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AdminController(SignInManager<ApplicationUser> signinManager, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("authenticated")]
        public ActionResult Authenticated()
        {
            var roles = string.Join(",", User.FindAll(ClaimTypes.Role).Select(u => u.Value));
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok($"USER: {user}, roles: {roles} ");
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<List<UserResponse>>> AccountList([FromQuery] PagingQuery model)
        {
            var users = await _userManager.Users.ToListAsync();
            var res = new List<UserResponse>();
            foreach (var user in users)
            {
                var resItem = new UserResponse(user);
                var roles = await _userManager.GetRolesAsync(user);
                resItem.SetRoles(roles.ToList());
                res.Add(resItem);
            }
            return res;
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<ActionResult<UserResponse>> Detail(string id)
        {
            var user = await _userManager.Users.Include(u => u.Employee).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return new UserResponse(user);
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.FindByNameAsync(_signinManager.Context.User.Identity.Name);
            if (user == null)
                return NotFound();
            var rs = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
            if (rs.Succeeded)
                return Ok();
            else
                return BadRequest(rs.Errors.Select(u => $"{u.Code}: {u.Description}"));
        }

        [HttpPost]
        [Route("user/{id}/reset")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return NotFound();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var password = GenerateRandomPassword(_userManager.Options.Password);
            var result = await _userManager.ResetPasswordAsync(user, token, password);

            if (result.Succeeded)
                return Ok();
            else
                return BadRequest(result.Errors.Select(u => new { code = $"{u.Code}: {u.Description}" }));
        }

        [HttpGet]
        [Route("user/{id}/roles")]
        public async Task<ActionResult<List<RoleResponse>>> Roles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest();

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.Where(e => roleNames.Contains(e.Name)).ToListAsync();
            return roles.Select(role => new RoleResponse(role)).ToList();
        }

        [HttpGet]
        [Route("roles")]
        public async Task<ActionResult<List<RoleResponse>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(role => new RoleResponse(role)).ToList();
        }

        [HttpGet]
        [Route("myroles")]
        public async Task<ActionResult<List<RoleResponse>>> GetMyRoles()
        {
            var user = await _userManager.GetUserAsync(User);
            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager.Roles.Where(e => roleNames.Contains(e.Name)).ToListAsync();
            return roles.Select(role => new RoleResponse(role)).ToList();
        }

        [HttpPut]
        [Route("user/{userId}/role/{role}")]
        public async Task<ActionResult> GrantUserRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.AddToRoleAsync(user, role);
            return Ok();
        }

        [HttpDelete]
        [Route("user/{userId}/role/{role}")]
        public async Task<ActionResult> RevokeUserRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.RemoveFromRoleAsync(user, role);
            return Ok();
        }

        private string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
        "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
        "abcdefghijkmnopqrstuvwxyz",    // lowercase
        "0123456789",                   // digits
        "!@$?_-"                        // non-alphanumeric
    };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
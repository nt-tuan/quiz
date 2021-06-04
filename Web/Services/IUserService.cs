using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ThanhTuan.Quiz.Entities;
using ThanhTuan.Quiz.Helpers;
using ThanhTuan.Quiz.Controllers.Models;
using ThanhTuan.Quiz.DBContext;
using ThanhTuan.Quiz.Repositories;
using System.Threading.Tasks;

namespace ThanhTuan.Quiz.Services
{
  public interface IUserService
  {
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<User> GetUserById(int id);
  }

  public class UserService : IUserService
  {
    // users hardcoded for simplicity, store in a db with hashed passwords in production applications

    private readonly AppSettings _appSettings;
    private readonly IUserRepository _repo;

    public UserService(IOptions<AppSettings> appSettings, IUserRepository repo)
    {
      _appSettings = appSettings.Value;
      _repo = repo;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
      var user = await _repo.GetUserByUsername(model.Username);
      // return null if user not found
      if (user == null || user.Password != model.Password) return null;
      // authentication successful so generate jwt token
      var token = generateJwtToken(user);
      return new AuthenticateResponse(user, token);
    }

    public async Task<User> GetUserById(int id)
    {
      return await _repo.GetUserById(id);
    }

    // helper methods
    private string generateJwtToken(User user)
    {
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()), new Claim("username", user.Username) }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}
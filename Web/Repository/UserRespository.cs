using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ThanhTuan.Quiz.Controllers.Models;
using ThanhTuan.Quiz.DBContext;
using ThanhTuan.Quiz.Entities;

namespace ThanhTuan.Quiz.Repositories
{
  public interface IUserRepository
  {
    Task<User> GetUserById(int id);
    Task<User> GetUserByUsername(string username);
    Task<IEnumerable<User>> GetAll();
  }

  public class UserRepository : IUserRepository
  {
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
      _dbContext = dbContext;
    }
    public async Task<User> GetUserById(int id)
    {
      var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
      return user;
    }

    public async Task<User> GetUserByUsername(string username)
    {
      var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Username == username);
      return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
      var users = await _dbContext.Users.ToListAsync();
      return users;
    }
  }
}
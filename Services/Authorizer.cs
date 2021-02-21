using Microsoft.AspNetCore.Http;

namespace ThanhTuan.Quiz.Services
{
  public class Authorizer
  {
    private readonly IHttpContextAccessor _accessor;
    public Authorizer(IHttpContextAccessor accessor)
    {
      _accessor = accessor;
    }

    public string GetUser()
    {
      return _accessor.HttpContext.Request.Headers["X-User"];
    }
  }
}
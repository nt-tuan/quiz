using System.Collections.Generic;

namespace dmc_auth.Controllers.Models
{
  public class ErrorResponse
  {
    public string message { get; set; }
    public List<string> messages { get; set; }
    public ErrorResponse()
    {
      message = "unexpected-error";
    }
    public ErrorResponse(string message)
    {
      this.message = message;
    }
  }

  public static class IDPErrors
  {
    public static ErrorResponse InvalidCredential = new ErrorResponse("invalid-credential");
    public static ErrorResponse UserNotFound = new ErrorResponse("user-not-found");
  }
}
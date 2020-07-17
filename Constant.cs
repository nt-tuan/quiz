using System;
namespace dmc_auth
{
  public class Constant
  {
    const string ENV_AUTH_URL = "AUTH_URL";
    const string ENV_REMEMBER_DURATION = "REMEMBER_DURATION";
    public static string GetAuthURL()
    {
      return Environment.GetEnvironmentVariable(ENV_AUTH_URL);
    }
    public static UInt64 GetRememberDuration()
    {
      UInt64 dur = 0;
      try
      {
        var str = Environment.GetEnvironmentVariable(ENV_REMEMBER_DURATION);
        dur = UInt64.Parse(str);
      }
      catch { }
      return dur;
    }
  }
}
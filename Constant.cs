using System;
namespace dmc_auth
{
    public class Constant
    {
        const string ENV_AUTH_URL = "AUTH_URL";
        const string ENV_REMEMBER_DURATION = "REMEMBER_DURATION";
        public const string ENV_PUBLIC_AUTH_URL = "PUBLIC_AUTH_URL";
        public const string ENV_CLIENT_ID = "CLIENT_ID";
        public const string ENV_CLIENT_SECRET = "CLIENT_SECRET";

        public static string GetUserInfoURL()
        {
            return Environment.GetEnvironmentVariable(ENV_PUBLIC_AUTH_URL) + "/userinfo";
        }
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
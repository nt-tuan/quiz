using System.Collections.Generic;
namespace dmc_auth.Middlewares
{
    public class IDTokenPayload
    {
        public string sub { get; set; }
        public string roles { get; set; }
    }
}
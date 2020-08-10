using System;

namespace dmc_auth.Controllers
{
    public class ConsentAcceptAccessTokenBody { }
    public class ConsentAcceptIDTokenBody
    {
        public string[] roles { get; set; }
        public string name { get; set; }
        public ConsentAcceptIDTokenBody(string[] roles, string name)
        {
            this.roles = roles;
            this.name = name;
        }
    }
    public class ConsentAcceptSessionBody
    {
        public ConsentAcceptAccessTokenBody access_token { get; set; }
        public ConsentAcceptIDTokenBody id_token { get; set; }
        public ConsentAcceptSessionBody(string[] roles, string name)
        {
            id_token = new ConsentAcceptIDTokenBody(roles, name);
            access_token = new ConsentAcceptAccessTokenBody();
        }
    }
    public class ConsentAcceptBody
    {
        public string[] grant_scope { get; set; }
        public bool remember { get; set; } = true;
        public ulong remember_for { get; set; } = Constant.GetRememberDuration();
        public ConsentAcceptSessionBody session { get; set; }
        public ConsentAcceptBody(string[] scope, string[] roles, string name)
        {
            grant_scope = scope;
            session = new ConsentAcceptSessionBody(roles, name);
        }
    }

    public class ConsentRequestBody
    {
        public string[] requested_scope { get; set; }
        public string subject { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace dmc_auth.Controllers
{
    public class LoginModel
    {
        public string login_challenge { get; set; }
        public string username { get; set; }
    }

    public class LoginInformationResponse
    {
        public string subject { get; set; }
        public bool skip { get; set; }
    }
    public class AcceptLoginRequest
    {
        public string acr { get; set; }
        public string force_subject_identifier { get; set; }
        public bool remember { get; set; }
        public Int64 remember_for { get; set; }
        public string subject { get; set; }

        public AcceptLoginRequest(string userId)
        {
            subject = userId;
            acr = "user-password";
            remember = true;
            remember_for = (Int64)Constant.GetRememberDuration();
        }
    }

    public class AcceptLoginResponse
    {
        public string redirect_to { get; set; }
    }

    public class AcceptConsentResponse
    {
        public string redirect_to { get; set; }
    }

    public class AcceptLogoutResponse
    {
        public string redirect_to { get; set; }
    }
}

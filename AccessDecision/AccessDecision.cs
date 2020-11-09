using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace dmc_auth.AccessDecision
{
    public class AccessDecision
    {
        private readonly List<Rule> Rules;
        public AccessDecision()
        {
            if (string.IsNullOrEmpty(Constant.RULES_FILEPATH))
            {
                throw new Exception();
            }
            using var reader = new StreamReader(Constant.RULES_FILEPATH);
            var content = reader.ReadToEnd();
            Rules = JsonSerializer.Deserialize<List<Rule>>(content);
        }
        public bool CanAccess(string path, string[] roles, string method)
        {
            foreach (var rule in Rules)
            {
                if (Regex.Match(path, rule.Path).Success)
                {
                    if (rule.Methods != null && rule.Methods.Length > 0 && !rule.Methods.Contains(method)) continue;
                    return roles.Contains(rule.Role);
                }
            }
            return false;
        }
    }
}

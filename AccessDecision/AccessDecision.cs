using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text.Json;
using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using dmc_auth.Hydra;
using dmc_auth.Hydra.Models;

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

    public Rule GetFirstMatchedRule(string path, string method)
    {
      foreach (var rule in Rules)
      {
        if (rule.Methods == null || rule.Methods.Count == 0 || !rule.Methods.Contains(method)) continue;
        if (Regex.Match(path, rule.Path).Success)
        {
          return rule;
        }
      }
      return null;
    }
  }
}

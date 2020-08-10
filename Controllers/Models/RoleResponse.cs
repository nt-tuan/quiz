using System.Collections.Generic;
using dmc_auth.Entities;

namespace dmc_auth.Controllers.Models
{
    public class RoleResponse
    {
        public string name { get; set; }
        public string description { get; set; }
        public RoleResponse(ApplicationRole entity)
        {
            name = entity.Name;
            description = entity.Description;
        }
    }
}
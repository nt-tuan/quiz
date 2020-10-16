using System.Collections.Generic;
using dmc_auth.Entities;

namespace dmc_auth.Controllers.Models
{
    public class RoleResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RoleResponse(ApplicationRole entity)
        {
            Name = entity.Name;
            Description = entity.Description;
        }
    }
}
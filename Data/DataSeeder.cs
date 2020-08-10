using System.Diagnostics;
using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using dmc_auth.Controllers.Models;
using dmc_auth.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace dmc_auth.Data
{
    public class DataSeeder
    {
        readonly ApplicationDbContext dbContext;
        readonly UserManager<ApplicationUser> userManager;
        readonly RoleManager<ApplicationRole> roleManager;
        public DataSeeder(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void Seed()
        {
            seedRoles();
            seedUsers();
        }

        private string readFile(string envKey)
        {
            var filepath = Environment.GetEnvironmentVariable(envKey);
            if (string.IsNullOrEmpty(filepath))
            {
                throw new Exception();
            }
            using (var reader = new StreamReader(filepath))
            {
                var content = reader.ReadToEnd();
                return content;
            }
        }

        private void seedRoles()
        {
            var content = readFile("INIT_ROLES_FILEPATH");
            var roles = JsonConvert.DeserializeObject<List<ApplicationRole>>(content);
            foreach (var role in roles)
            {
                var entity = roleManager.FindByNameAsync(role.Name).Result;
                if (entity == null)
                {
                    roleManager.CreateAsync(role).Wait();
                }
            }

        }
        private void seedUsers()
        {
            var content = readFile("INIT_USERS_FILEPATH");
            var users = JsonConvert.DeserializeObject<List<CreateUserModel>>(content);
            foreach (var user in users)
            {
                var entity = userManager.FindByNameAsync(user.Username).Result;
                if (entity == null)
                {
                    var result = userManager.CreateAsync(new ApplicationUser { UserName = user.Username, Email = user.Email }, user.Password).Result;
                    if (result.Succeeded)
                    {
                        entity = userManager.FindByNameAsync(user.Username).Result;
                    }
                }
                userManager.AddToRolesAsync(entity, user.Roles).Wait();
            }
        }
    }
}
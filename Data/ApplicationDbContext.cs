using dmc_auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dmc_auth.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, String>
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public ApplicationDbContext(
            DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasOne(u => u.Department).WithMany().HasForeignKey(u => u.DepartmentId);
            modelBuilder.Entity<Department>().HasOne(u => u.Manager).WithMany().HasForeignKey(u => u.ManagerId);
            modelBuilder.Entity<Department>().HasOne(u => u.Parent).WithMany().HasForeignKey(u => u.ParentId);
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}

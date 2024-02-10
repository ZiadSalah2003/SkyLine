using Microsoft.EntityFrameworkCore;
using skyline.Models;
using System.ComponentModel.DataAnnotations;

namespace skyline.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get;set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}

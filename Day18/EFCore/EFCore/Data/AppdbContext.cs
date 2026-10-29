using EFCore.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.Data
{
    public class AppdbContext : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.
                UseSqlServer(@"Server=AJITH\SQLEXPRESS;Database= StudentCourseEFCoreDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}

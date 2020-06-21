using FBAutomation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FBAutomation.DAL
{
    public class FBContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Assigment> Assigments { get; set; }

        public DbSet<Group> Groups { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=FaceBookDB;Trusted_Connection=True;");
        }
      
    }
}

using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAPI.DataAccess
{
    public class LoginDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        // ---------------------------Create DataBase--------------------------------
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "server=localhost; Database=NewsDb; User Id=Damith; Password=omega_1234";
            optionsBuilder.UseSqlServer(connectionString);

        }
        // ---------------------------Add Dummy Data--------------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User[]
            {
                new User{ Id = 10001, Name = "Test Admin", Email = "admin@gmail.com", PasswordHash =  Encoding.ASCII.GetBytes("1234"), PasswordSalt =  Encoding.ASCII.GetBytes("1234") },
            });
        }

    }
}

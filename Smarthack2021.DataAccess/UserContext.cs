using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smarthack2021.Core.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smarthack2021.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public DbSet<PasswordObject> Passwords { get; set; }

        public DbSet<CryptographicalKeyObject> Keys { get; set; }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Passwords)
                .WithOne(p => p.User);
            
            modelBuilder.Entity<User>()
                .HasMany(u => u.Keys)
                .WithOne(k => k.User);
        }
    }
}

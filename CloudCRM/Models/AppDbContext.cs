using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CloudCRM.Models
{

    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //builder.Entity<ApplicationUser>().Property(p => p.Id).HasMaxLength(36);
            builder.Entity<IdentityUserLogin<int>>().HasKey(u => u.ProviderKey);
            builder.Entity<IdentityUserLogin<int>>().Property(p => p.UserId).HasMaxLength(36);
            builder.Entity<IdentityUserLogin<int>>().Property(p => p.LoginProvider).HasMaxLength(255);
            builder.Entity<IdentityUserLogin<int>>().Property(p => p.ProviderKey).HasMaxLength(36);
            builder.Entity<IdentityUserLogin<int>>().Property(p => p.ProviderDisplayName).HasMaxLength(255);

            builder.Entity<IdentityUserToken<int>>().Property(p => p.LoginProvider).HasMaxLength(255);
            builder.Entity<IdentityUserToken<int>>().Property(p => p.Name).HasMaxLength(255);
        }

        //public DbSet<Person> Persons { get; set; }

    }

}

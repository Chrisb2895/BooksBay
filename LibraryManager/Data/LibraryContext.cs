using LibraryManager.Models;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace LibraryManager.Data
{
    public class LibraryContext : IdentityDbContext   //Step 1 for identity auth
    {
        public static readonly ILoggerFactory _LoggerFactory = LoggerFactory.Create(builder => builder.AddLog4Net());

        public LibraryContext(DbContextOptions<LibraryContext> opt): base (opt)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_LoggerFactory.AddLog4Net());
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //builder.Seed();
        }

        public DbSet<Library>  Libraries { get; set; }
    }
}

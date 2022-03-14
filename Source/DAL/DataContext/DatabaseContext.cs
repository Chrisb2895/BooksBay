using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.Extensions.Logging;

namespace DAL.DataContext
{
    public class DatabaseContext : DbContext
    {
        public static OptionsBuild Options = new OptionsBuild();
        public DbSet<Library> Libraries { get; set; }

        public static readonly ILoggerFactory _LoggerFactory = LoggerFactory.Create(builder => builder.AddLog4Net());

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_LoggerFactory.AddLog4Net());
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Library>().ToTable("Libraries");
            modelBuilder.Entity<Library>().Property(lib=> lib.Library_CreationDate).IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Library>().Property(lib => lib.Library_ModifiedDate).IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Library>().Property(lib => lib.Enabled).IsRequired(true)
                .HasDefaultValue(true);
       
        }


        public class OptionsBuild
        {
            public AppConfiguration Settings { get; private set; }

            public DbContextOptionsBuilder<DatabaseContext> OptionsBuilder { get; private set; }
            public DbContextOptions<DatabaseContext> DatabaseOptions { get; private set; }

            public OptionsBuild()
            {
                Settings = new AppConfiguration();
                OptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
                OptionsBuilder.UseSqlServer(Settings.SqlConnectionString);
                DatabaseOptions = OptionsBuilder.Options;
            }

            
        }
    }
}

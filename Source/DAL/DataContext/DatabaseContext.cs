using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DAL.DataContext
{
    public class DatabaseContext : IdentityDbContext
    {
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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Library>().ToTable("Libraries");
            modelBuilder.Entity<Library>().Property(lib => lib.Library_CreationDate).IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Library>().Property(lib => lib.Library_ModifiedDate).IsRequired(true)
                .HasDefaultValue(DateTime.UtcNow);
            modelBuilder.Entity<Library>().Property(lib => lib.Enabled).IsRequired(true)
                .HasDefaultValue(true);

        }



    }
}

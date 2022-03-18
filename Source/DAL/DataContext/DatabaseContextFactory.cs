using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContext
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {
        public DatabaseContext CreateDbContext(string[] args, IConfiguration configuration)
        {
            AppConfiguration Settings = new AppConfiguration(configuration);
            DbContextOptionsBuilder<DatabaseContext> OptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            OptionsBuilder.UseSqlServer(Settings.SqlConnectionString);
            return new DatabaseContext(OptionsBuilder.Options);
        }
    }
}

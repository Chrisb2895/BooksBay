using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DataContext
{
    internal class DatabaseContext : DbContext
    {
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

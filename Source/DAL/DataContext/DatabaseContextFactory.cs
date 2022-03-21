using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DAL.DataContext
{
    public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
    {

        public DatabaseContextFactory()
        {

        }

        public DatabaseContext CreateDbContext(string[] args)
        {
            AppConfiguration Settings = new AppConfiguration();
            DbContextOptionsBuilder<DatabaseContext> OptionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            OptionsBuilder.UseSqlServer(Settings.SqlConnectionString);
            return new DatabaseContext(OptionsBuilder.Options);
        }



    }
}

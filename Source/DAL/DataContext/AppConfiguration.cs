using DAL.CustomProviders;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL.DataContext
{
    public class AppConfiguration
    {
        public string SqlConnectionString { get; private set; }

        public AppConfiguration(IConfiguration configuration)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            configurationBuilder.AddEncryptedProvider(configuration);
            IConfigurationRoot root = configurationBuilder.Build();
            IConfigurationSection appSettings = root.GetSection("ConnectionStrings:LibraryConn");
            var conStrBuilder = new SqlConnectionStringBuilder(root.GetConnectionString("LibraryConn"));
            conStrBuilder.Password = root.GetValue<string>("dbPWD");
            var connString = "";
            connString = conStrBuilder.ConnectionString;
            SqlConnectionString = connString;
        }


    }
}

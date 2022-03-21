using DAL.CustomProviders;
using DAL.StaticClasses;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DAL.DataContext
{
    public class AppConfiguration
    {
        public string SqlConnectionString { get; private set; }

        public AppConfiguration()
        {
            /*ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            configurationBuilder.AddEncryptedProvider(AppData.Configuration);
            IConfigurationRoot root = configurationBuilder.Build();
            IConfigurationSection appSettings = root.GetSection("ConnectionStrings:LibraryConn");*/
            var conStrBuilder = new SqlConnectionStringBuilder(AppData.Configuration.GetConnectionString("LibraryConn"));
            conStrBuilder.Password = AppData.Configuration.GetValue<string>("dbPWD");
            var connString = "";
            connString = conStrBuilder.ConnectionString;
            SqlConnectionString = connString;
        }


    }
}

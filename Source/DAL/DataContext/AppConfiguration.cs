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
            var conStrBuilder = new SqlConnectionStringBuilder(AppData.Configuration.GetConnectionString("LibraryConn"));
            conStrBuilder.Password = AppData.Configuration.GetValue<string>("dbPWD");
            var connString = "";
            connString = conStrBuilder.ConnectionString;
            SqlConnectionString = connString;
        }


    }
}

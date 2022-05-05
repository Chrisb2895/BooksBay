using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DAL.CustomProviders
{
    public static class CustomConfigProviderExtensions
    {
        public static IConfigurationBuilder AddEncryptedProvider(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            return builder.Add(new CustomConfigProvider(configuration));
        }

        public static string GetConnectionStringExt(this IConfiguration configuration)
        {
            var conStrBuilder = new SqlConnectionStringBuilder(configuration.GetConnectionString("LibraryConn"));
            conStrBuilder.Password = configuration.GetValue<string>("dbPWD");
            return conStrBuilder.ConnectionString;
        }
    }
}

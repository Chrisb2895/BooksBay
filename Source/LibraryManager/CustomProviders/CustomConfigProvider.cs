using LibraryManager.Helpers;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace LibraryManager.CustomProviders
{
    public class CustomConfigProvider : ConfigurationProvider, IConfigurationSource
    {
        public CustomConfigProvider()
        {
        }

        public override void Load()
        {
            Data = UnencryptMyConfiguration(base.Data);
        }

        private IDictionary<string, string> UnencryptMyConfiguration(IDictionary<string, string> bData)
        {
            // do whatever you need to do here, for example load the file and unencrypt key by key
            //Like:
            var configValues = new Dictionary<string, string>();
            string encConn = "";
            CryptoHelper crypt = new CryptoHelper();
            if (bData.TryGetValue("LibraryConn", out encConn))
            {
                var conStrBuilder = new SqlConnectionStringBuilder(encConn);
                conStrBuilder.Password = crypt.GetUnCrypted(conStrBuilder.Password);
                configValues.Add("LibraryConn", conStrBuilder.ConnectionString);


            }
            return configValues;
        }

        private IDictionary<string, string> CreateAndSaveDefaultValues(IDictionary<string, string> defaultDictionary)
        {
            var configValues = new Dictionary<string, string>
            {
                {"key1", "encryptedValue1"},
                {"key2", "encryptedValue2"}
            };
            return configValues;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomConfigProvider();
        }
    }
}

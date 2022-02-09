using LibraryManager.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace LibraryManager.CustomProviders
{
    public class CustomConfigProvider : ConfigurationProvider, IConfigurationSource
    {
        IConfiguration _configuration;
        public CustomConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void Load()
        {
            Data = UnencryptMyConfiguration();
        }

        private IDictionary<string, string> UnencryptMyConfiguration()
        {
            // do whatever you need to do here, for example load the file and unencrypt key by key
            //Like:           
            var decr = CryptoHelper.GetUnCrypted(_configuration["DbPassword"], _configuration["MasterPWD"]);           
            var configValues = new Dictionary<string, string>
                   {
                        {"dbPWD", decr},
                        {"key2", "unencryptedValue2"}
                   };
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
            return new CustomConfigProvider(_configuration);
        }
    }
}


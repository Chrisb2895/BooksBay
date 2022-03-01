using LibraryManager.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace LibraryManager.CustomProviders
{
    public class CustomConfigProvider : ConfigurationProvider, IConfigurationSource
    {
        public readonly IConfiguration _configuration;
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
            var decr = CryptoHelper.GetUnCrypted(Encoding.Default.GetString(Convert.FromBase64String(_configuration["DbPassword"])), _configuration["MasterPWD"]);           
            var configValues = new Dictionary<string, string>
                   {
                        {"dbPWD", decr},
                        {"MasterPWD", _configuration["MasterPWD"]}
                   };
            return configValues;
        }

        

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomConfigProvider(_configuration);
        }
    }
}


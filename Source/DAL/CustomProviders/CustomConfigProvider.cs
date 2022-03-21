using DAL.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.CustomProviders
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
            base.Load();
            Data = UnencryptMyConfiguration();
        }

        private IDictionary<string, string> UnencryptMyConfiguration()
        {
            // do whatever you need to do here, for example load the file and unencrypt key by key
            //Like:           
            var decrDBPwd = CryptoHelper.GetUnCrypted(Encoding.Default.GetString(Convert.FromBase64String(_configuration["DbPassword"])), _configuration["MasterPWD"]);
            var decrClientSecret = CryptoHelper.GetUnCrypted(Encoding.Default.GetString(Convert.FromBase64String(_configuration["ExternalGoogleAuthInfos:ClientSecret"])), _configuration["MasterPWD"]);
            var decrAppSecret = CryptoHelper.GetUnCrypted(Encoding.Default.GetString(Convert.FromBase64String(_configuration["ExternalFacebookAuthInfos:AppSecret"])), _configuration["MasterPWD"]);
            var configValues = new Dictionary<string, string>
                   {
                        {"dbPWD", decrDBPwd},
                        {"ggClientSecret", decrClientSecret},
                        {"fbAppSecret", decrAppSecret},
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


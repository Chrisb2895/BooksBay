using DAL.Helpers;
using Microsoft.Extensions.Configuration;
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
            var decrDBPwd = CryptoHelper.DecryptData(_configuration["PrivateKey"], _configuration["DbPassword"]);
            var decrClientSecret = CryptoHelper.DecryptData(_configuration["PrivateKey"], _configuration["ExternalGoogleAuthInfos:ClientSecret"]);           
            var configValues = new Dictionary<string, string>
                   {
                        {"dbPWD", decrDBPwd},
                        {"ggClientSecret", decrClientSecret},
                        {"PrivateKey", _configuration["PrivateKey"]}
                   };
            return configValues;
        }



        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new CustomConfigProvider(_configuration);
        }
    }
}


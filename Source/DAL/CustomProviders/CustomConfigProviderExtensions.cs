using Microsoft.Extensions.Configuration;

namespace DAL.CustomProviders
{
    public static class CustomConfigProviderExtensions
    {
        public static IConfigurationBuilder AddEncryptedProvider(this IConfigurationBuilder builder, IConfiguration configuration)
        {
            return builder.Add(new CustomConfigProvider(configuration));
        }
    }
}

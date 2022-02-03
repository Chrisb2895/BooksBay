using Microsoft.Extensions.Configuration;

namespace LibraryManager.CustomProviders
{
    public static class CustomConfigProviderExtensions
    {
        public static IConfigurationBuilder AddEncryptedProvider(this IConfigurationBuilder builder)
        {
            return builder.Add(new CustomConfigProvider());
        }
    }
}

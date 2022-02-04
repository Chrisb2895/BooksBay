using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;

namespace LibraryManager.CustomProviders
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddProtectedConfiguration(this IServiceCollection services)
        {
            services
                .AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"c:\keys"))
                .ProtectKeysWithDpapi();

            return services;
        }

        public static IServiceCollection ConfigureProtected<TOptions>(this IServiceCollection services, IConfigurationSection section) where TOptions : class, new()
        {
            return services.AddSingleton(provider =>
            {
                var dataProtectionProvider = provider.GetRequiredService<IDataProtectionProvider>();
                section = new ProtectedConfigurationSection(dataProtectionProvider, section);

                var options = section.Get<TOptions>();
                return Options.Create(options);
            });
        }
    }

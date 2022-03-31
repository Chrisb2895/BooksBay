using DAL.CustomProviders;
using DAL.StaticClasses;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace LibraryManager
{
    public static class Program
    {
        private static ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            try
            {

                XmlDocument log4netConfig = new XmlDocument();
                log4netConfig.Load(File.OpenRead("log4net.config"));

                var repo = LogManager.CreateRepository( Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

                log.Warn("Application - Main Started");

                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {

                    //IdentityServer configure step
                    //AddConfigurationDataToDatabase(scope);

                }

                host.Run();


            }
            catch (Exception ex)
            {
                log.ErrorFormat("Errore in LibraryManager WebAPI Program: {0}  \r\n {1} \n\r InnerEx: {2}", ex.Message, ex.StackTrace, ex.InnerException);
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>().ConfigureLogging((hostingContext, logging) =>
                    {
                        logging.AddLog4Net();

                    });

                })
                .ConfigureAppConfiguration((hostingContext, config) => {
                
                    var decryptedConfig = config
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddEncryptedProvider(hostingContext.Configuration).Build();
                    AppData.Configuration = decryptedConfig;
                });

        private static void AddConfigurationDataToDatabase(IServiceScope scope)
        {
            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    if (!context.Clients.Any())
                    {
                        foreach (var client in Configuration.GetClients())
                        {
                            context.Clients.Add(client.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.IdentityResources.Any())
                    {
                        foreach (var resource in Configuration.GetIdentityResources())
                        {
                            context.IdentityResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }

                    if (!context.ApiScopes.Any())
                    {
                        foreach (var resource in Configuration.GetApis())
                        {
                            context.ApiResources.Add(resource.ToEntity());
                        }
                        context.SaveChanges();
                    }
        }

    }
}

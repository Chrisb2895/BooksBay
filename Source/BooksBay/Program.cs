using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManager;
using log4net;
using System.Xml;
using System.Reflection;
using System.IO;

namespace BooksBay
{

    /*SOLUTION TO DOS THINGS TO ADJUST:
    1) LIBRARY MANAGER AUTH CONTROLLER, CONFIGURE ROUTE WITHOUT NEED TO REPEAT CONTROLLER NAME
    2) SECURE SECRETS STRINGS
    3)
    */

    public static class Program
    {
        private static ILog log = LogManager.GetLogger(typeof(Program));
        public static void Main(string[] args)
        {
            try
            {

                XmlDocument log4netConfig = new XmlDocument();
                log4netConfig.Load(File.OpenRead("log4net.config"));
                var repo = LogManager.CreateRepository(
                    Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                log.Warn("Application - Main is invoked");

                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Errore in BooksBay Program: {0}  \r\n {1}", ex.Message, ex.StackTrace);
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

                });
    }
}

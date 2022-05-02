using LibraryManagerAPI;
using log4net;
using System.Diagnostics;
using System.Reflection;
using System.Xml;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    ConfigureLog4Net();

    using (var scope = app.Services.CreateScope())
    {

        //IdentityServer configure step : inserts data in DB only to be done first time (app install)
        //Configuration.AddConfigurationDataToDatabase(scope);

    }



}
catch (Exception ex)
{
    using (EventLog eventLog = new EventLog("Application"))
    {
        eventLog.Source = "Application";
        eventLog.WriteEntry(String.Format("Web Application LibraryManagerAPI Start Exception {0} {1}",ex.Message,ex.StackTrace), EventLogEntryType.Error, 101, 1);
    }
}

 void ConfigureLog4Net()
{
    XmlDocument log4netConfig = new XmlDocument();
    log4netConfig.Load(File.OpenRead("log4net.config"));

    var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
    log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
}

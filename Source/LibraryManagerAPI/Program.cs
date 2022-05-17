using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using DAL.CoreAdminExtensions;
using DAL.CustomProviders;
using DAL.DataContext;
using LOGIC.Services.Implementation;
using LOGIC.Services.Interfaces;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Serialization;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Logging.AddLog4Net("log4net.config");

    //Read passwords from secret configuration and unencrypt them and add them to be used from application
    builder.Configuration.AddEncryptedProvider(builder.Configuration);
    var connString = builder.Configuration.GetConnectionStringExt();
    builder.Services.AddDbContext<DatabaseContext>(opt =>
    {
        opt.UseSqlServer(connString);
    });
    builder.Services.AddSingleton<CustomConfigProvider>();
    builder.Services.AddScoped<ILibraryService, LibraryService>();
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    //should be only API but use it also as Web Site cause of IdentityServer (some classes are not deserializable with json.... so the login is handled by API)
    builder.Services.AddControllersWithViews().AddNewtonsoftJson(s =>
    {

        s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

    });

    #region IdentityServer Authentication Handler

    //IDServer step 2
    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 10;
        options.Password.RequiredUniqueChars = 3;
    }).AddEntityFrameworkStores<DatabaseContext>()
    .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(config =>
    {
        config.Cookie.Name = "IdentityServer.Cookie";
        config.LoginPath = "/Auth/Login";
        config.LogoutPath = "/Auth/Logout";
    });

    var assembly = typeof(Program).Assembly.GetName().Name;

    var certPath = Path.Combine(builder.Environment.ContentRootPath, "libManager.pfx");

    var certificate = new X509Certificate2(certPath, "XCertificate");

    builder.Services.AddIdentityServer()
        .AddAspNetIdentity<IdentityUser>()
        .AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(connString,
                sql => sql.MigrationsAssembly(assembly));
        })
        .AddOperationalStore(options =>
        {
            options.ConfigureDbContext = b => b.UseSqlServer(connString,
                sql => sql.MigrationsAssembly(assembly));
        })
        .AddSigningCredential(certificate);

    //External Login FB Step 1, poi per configurare appid --> https://developers.facebook.com/docs/facebook-login/
    /*services.AddAuthentication().AddFacebook(config=>
    {
        config.AppId = "";
        config.AppSecret = "";

    });*/

    //https://console.cloud.google.com/apis/dashboard?hl=IT&ref=https:%2F%2Fwww.google.com%2F&pli=1&project=books-bay-web-site-sts
    builder.Services.AddAuthentication().AddGoogle(config =>
    {
        config.ClientId = builder.Configuration.GetSection("ExternalGoogleAuthInfos").GetValue<string>("ClientId");
        config.ClientSecret = builder.Configuration.GetValue<string>("ggClientSecret");
    });

    #endregion

    //OWASP SECURING
    builder.Services.AddResponseCaching();
    builder.Services.AddMvc(options =>
    {
        options.CacheProfiles.Add("default", new CacheProfile
        {
            Duration = 3600,
            Location = ResponseCacheLocation.Any,
            NoStore = true
        });

    });
    //END OWASP SECURING

    builder.Services.AddCoreAdmin(new CoreAdminOptions() { IgnoreEntityTypes = new List<Type>() { typeof(IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext) } });


    var app = builder.Build();
    

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    //OWASP SECURING
    app.UseStaticFiles(new StaticFileOptions // for wwwroot files
    {
        OnPrepareResponse = (context) =>
        {
            var headers = context.Context.Response.GetTypedHeaders();

            headers.CacheControl = new CacheControlHeaderValue
            {
                Public = true,
                MaxAge = TimeSpan.FromDays(30)
            };

            headers.Headers.Remove("X-Powered-By");
        }
    });

    app.UseRouting();

    //OWASP SECURING

    app.UseResponseCaching();

    app.Use((context, next) =>
    {
        context.Response.GetTypedHeaders().CacheControl =
              new CacheControlHeaderValue()
              {
                  NoCache = false,
                  NoStore = false,
                  MustRevalidate = false
              };
        context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

        if (context.Request.Headers.TryGetValue("X-Forwarded-Prefix", out var value))
            context.Request.PathBase = value.First();

        return next();
    });

    app.UseCookiePolicy(new CookiePolicyOptions
    {
        HttpOnly = HttpOnlyPolicy.Always,
        Secure = CookieSecurePolicy.Always,
        //MinimumSameSitePolicy = SameSiteMode.Strict
        //TO PRD NOTES: this comment up here should run in production but for dev and debug with local host
        MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax
    });

    //END OWASP SECURING

    //IDServer step 1
    app.UseIdentityServer();
    app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
        endpoints.MapControllers();

    });


    app.MapControllers();

    app.Run();

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
        eventLog.WriteEntry(String.Format("Web Application LibraryManagerAPI Start Exception {0} {1}", ex.Message, ex.StackTrace), EventLogEntryType.Error, 101, 1);
    }
}



using LibraryManager.Classes.Controllers;
using LibraryManager.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace LibraryManager
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        //SOLUTION THINGS TO ADJUST:
        //1) PUT ALL HARDCODED STRING IN APPSETTING.JSON
        //2) LIBRARY MANAGER AUTH CONTROLLER, CONFIGURE ROUTE WITHOUT NEED TO REPEAT CONTROLLER NAME


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            var connString = Configuration.GetConnectionString("LibraryConn");
            services.AddDbContext<LibraryContext>(opt => { opt.UseSqlServer(connString); });

            //IDServer step 2
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<LibraryContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IdentityServer.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            var assembly = typeof(Startup).Assembly.GetName().Name;

            var certPath = Path.Combine(Env.ContentRootPath, "libManager.pfx");

            var certificate = new X509Certificate2(certPath, "XCertificate");

            services.AddIdentityServer()
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

            services.AddAuthentication().AddGoogle(config =>
           {
               config.ClientId = Configuration.GetSection("ExternalGoogleAuthInfos").GetValue<string>("ClientId");
               config.ClientSecret = Configuration.GetSection("ExternalGoogleAuthInfos").GetValue<string>("ClientSecret");
           });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryManager", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ILibraryRepo, SqlLibraryRepo>();


            
            services.AddControllersWithViews(config =>
            {
                
            }).AddNewtonsoftJson(s => { 

                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); 
            
            });
            services.AddRazorPages();


            //OWASP SECURING
            services.AddResponseCaching();
            services.AddMvc(options =>
            {
                options.CacheProfiles.Add("default", new CacheProfile
                {
                    Duration = 30,
                    Location = ResponseCacheLocation.Any,
                    NoStore = true
                });
            });
            //END OWASP SECURING

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("../swagger/v1/swagger.json", "LibraryManager v1"));
            }

            app.UseHttpsRedirection();

            //OWASP SECURING
            app.UseStaticFiles(new StaticFileOptions // for wwwroot files
            {
                OnPrepareResponse = (context) =>
                {
                    var headers = context.Context.Response.GetTypedHeaders();

                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
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
                      new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                      {
                          NoCache = true,
                          NoStore = true,
                          MustRevalidate = true
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
                MinimumSameSitePolicy = SameSiteMode.Strict
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


        }
    }
}

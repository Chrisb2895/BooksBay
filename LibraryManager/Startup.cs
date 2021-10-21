using LibraryManager.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace LibraryManager
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env= env;
        }

        //SOLUTION THINGS TO ADJUST:
        //1) PUT ALL HARDCODED STRING IN APPSETTING.JSON
        //2) LIBRARY MANAGER AUTH CONTROLLER, CONFIGURE ROUTE WITHOUT NEED TO REPEAT CONTROLLER NAME
        //3) BOOKSBAY MVC CLIENT ADD LOG4NET AS DONE IN LIBRARY MANAGER
       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

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

            var certificate = new X509Certificate2(certPath,"XCertificate");

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
                config.AppId = "839658230062666";
                config.AppSecret = "2e309b81510419e8db96494b6f91cd5f";
            
            });*/

            services.AddAuthentication().AddGoogle(config =>
           {
               config.ClientId = "1073258207424-2f18mkdfvdntsvhgh98dc1csiklbnubf.apps.googleusercontent.com";
               config.ClientSecret = "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";
           });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryManager", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ILibraryRepo, SqlLibraryRepo>();

           
            services.AddRazorPages();
            services.AddControllersWithViews().AddNewtonsoftJson(s => { s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManager v1"));
            }


            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

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

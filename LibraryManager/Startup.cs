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
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManager
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       

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
                //.AddInMemoryApiResources(LibraryManager.Configuration.GetApis())
                //.AddInMemoryIdentityResources(LibraryManager.Configuration.GetIdentityResources())
                //.AddInMemoryClients(LibraryManager.Configuration.GetClients())                
                .AddDeveloperSigningCredential();
           
            

            //services.AddAuthentication();

            //IDServer step 4
            /*services.AddAuthentication()
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = "https://localhost:44326/";
                    //usare nome corretto in progetto booksbay c'è configuration getapi
                    config.Audience = "ApiOne";
                });*/

            services.AddHttpClient();


            //services.AddControllers().AddNewtonsoftJson(s => { s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); });
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

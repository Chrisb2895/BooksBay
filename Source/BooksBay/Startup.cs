using BooksBay.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BooksBay
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly string _API_Endpoint;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _API_Endpoint = Configuration.GetValue<string>("WebAPI_Endpoint");
        }
      
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSingleton<IConfiguration>(Configuration);

            //IDServer step 6
            services.AddAuthentication(config =>
           {
               config.DefaultScheme = "Cookie";
               config.DefaultChallengeScheme = "oidc";

           })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config =>
                {
                    //questo punta al progetto LibraryManager
                    config.Authority = _API_Endpoint;
                    config.ClientId = "client_id_mvc";
                    config.ClientSecret = "client_secret_mvc";
                    config.SaveTokens = true;
                    config.ResponseType = "code";
                    config.SignedOutCallbackPath = "/Home/Index";
                    config.GetClaimsFromUserInfoEndpoint = true;
                    config.Scope.Add("openid");
                    config.Scope.Add("profile");               
                    config.Scope.Add("offline_access");

                }).AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters.RoleClaimType = "roles";
                    opt.TokenValidationParameters.NameClaimType = System.Security.Claims.ClaimTypes.Name;
                });


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddTransient<LibraryAPI>();

            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            //IDServer step 5

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

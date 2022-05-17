using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;


try
{
    var builder = WebApplication.CreateBuilder(args);

    string _API_Endpoint = builder.Configuration.GetValue<string>("WebAPI_Endpoint");

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    //IDServer step 6
    builder.Services.AddAuthentication(config =>
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

    builder.Services.AddHttpClient();

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

    builder.Logging.AddLog4Net("log4net.config");




    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();

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
        MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict
    });

    //END OWASP SECURING

    app.UseRouting();

    //IDServer step 5
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    //area handle
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        endpoints.MapRazorPages();
    });

    app.Run();

}
catch (Exception ex)
{
    using (EventLog eventLog = new EventLog("Application"))
    {
        eventLog.Source = "Application";
        eventLog.WriteEntry(String.Format("Web Application BooksBay Start Exception {0} {1}", ex.Message, ex.StackTrace), EventLogEntryType.Error, 101, 1);
    }
}

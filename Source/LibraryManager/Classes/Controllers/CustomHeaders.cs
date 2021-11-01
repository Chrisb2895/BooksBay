using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LibraryManager.Classes.Controllers
{

    public class CustomHeaders : IAsyncActionFilter
    {
        /*[ResponseCache(Location = ResponseCacheLocation.None)]
        public override void OnActionExecuted(System.Web.Mvc.ActionExecutedContext context)
        {
            context.act
            context.RequestContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.RequestContext.HttpContext.Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            context.RequestContext.HttpContext.Response.AppendHeader("Pragma", "no-cache");
            context.RequestContext.HttpContext.Response.AppendHeader("Expires", "0");

            base.OnActionExecuted(context);
        }*/

        public async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            var result = await next();
        }
    }
}

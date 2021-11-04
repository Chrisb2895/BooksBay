using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;


namespace LibraryManager.Classes.Controllers
{

    public class CustomHeaders : IAsyncActionFilter
    {
  
        public async Task OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            await next();
        }
    }
}

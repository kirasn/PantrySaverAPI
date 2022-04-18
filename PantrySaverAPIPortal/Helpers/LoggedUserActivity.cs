using Microsoft.AspNetCore.Mvc.Filters;
using PantrySaverAPIPortal.Extensions;

namespace PantrySaverAPIPortal.Helpers
{
    public class LoggedUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userName = resultContext.HttpContext.User.GetUserName();
        }
    }
}
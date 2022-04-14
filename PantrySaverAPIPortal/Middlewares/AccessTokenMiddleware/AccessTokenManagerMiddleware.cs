using System.Net;
using PantrySaverAPIPortal.Services.AuthenticationServices;

namespace PantrySaverAPIPortal.Middlewares.AccessTokenMiddleware
{
    public class AccessTokenManagerMiddleware : IAccessTokenManagerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IAccessTokenManager accessTokenManager;

        public AccessTokenManagerMiddleware(RequestDelegate next, IAccessTokenManager accessTokenManager)
        {
            this.next = next;
            this.accessTokenManager = accessTokenManager;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (await accessTokenManager.IsCurrentActiveToken())
            {
                await next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
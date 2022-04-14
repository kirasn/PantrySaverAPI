namespace PantrySaverAPIPortal.Middlewares.AccessTokenMiddleware
{
    public static class AccessTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenManagerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AccessTokenManagerMiddleware>();
        }
    }
}
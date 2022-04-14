namespace PantrySaverAPIPortal.Middlewares.AccessTokenMiddleware
{
    public interface IAccessTokenManagerMiddleware
    {
        Task InvokeAsync(HttpContext context);
    }
}
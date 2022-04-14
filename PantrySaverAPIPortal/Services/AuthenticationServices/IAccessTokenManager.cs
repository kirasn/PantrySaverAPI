using PantrySaver.Models;

namespace PantrySaverAPIPortal.Services.AuthenticationServices
{
    public interface IAccessTokenManager
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
        Task<bool> IsCurrentActiveToken();
        Task<bool> IsActiveAsync(string token);
    }
}
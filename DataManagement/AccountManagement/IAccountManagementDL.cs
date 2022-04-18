using PantrySaver.Models;

namespace DataManagement.AccountManagement
{
    public interface IAccountManagementDL
    {
        Task<ApplicationUser> GetProfile(string userName);
        Task<ApplicationUser> UpdateProfile(ApplicationUser userProfile);
    }
}
using PantrySaver.Models;

namespace BussinessManagement.AccountManagement
{
    public interface IAccountManagementBL
    {
        Task<ApplicationUser> GetProfile(string userName);
        Task<ApplicationUser> UpdateProfile(ApplicationUser userProfile);
    }
}
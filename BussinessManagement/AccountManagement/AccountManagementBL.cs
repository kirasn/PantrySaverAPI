using DataManagement.AccountManagement;
using PantrySaver.Models;

namespace BussinessManagement.AccountManagement
{
    public class AccountManagementBL : IAccountManagementBL
    {
        private readonly IAccountManagementDL _repo;

        public AccountManagementBL(IAccountManagementDL repo)
        {
            _repo = repo;
        }

        public async Task<ApplicationUser> GetProfile(string userName)
        {
            return await _repo.GetProfile(userName);
        }

        public async Task<ApplicationUser> UpdateProfile(ApplicationUser userProfile)
        {
            return await _repo.UpdateProfile(userProfile);
        }
    }
}
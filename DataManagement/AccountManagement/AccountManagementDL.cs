using Microsoft.EntityFrameworkCore;
using PantrySaver.Models;

namespace DataManagement.AccountManagement
{
    public class AccountManagementDL : IAccountManagementDL
    {
        private readonly PantrySaverContext _context;

        public AccountManagementDL(PantrySaverContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetProfile(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.UserName.Equals(userName));
        }

        public async Task<ApplicationUser> UpdateProfile(ApplicationUser userProfile)
        {
            var userFromDB = await _context.Users.FirstOrDefaultAsync(p => p.UserName.Equals(userProfile.UserName));

            userFromDB.FirstName = userProfile.FirstName;
            userFromDB.LastName = userProfile.LastName;
            userFromDB.DateOfBirth = userProfile.DateOfBirth;
            await _context.SaveChangesAsync();
            return userFromDB;
        }
    }
}
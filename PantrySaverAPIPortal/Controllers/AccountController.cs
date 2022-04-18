using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessManagement.AccountManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PantrySaver.Models;
using PantrySaverAPIPortal.Consts;
using PantrySaverAPIPortal.DataTransferObjects;
using PantrySaverAPIPortal.Extensions;
using PantrySaverAPIPortal.Helpers;

namespace PantrySaverAPIPortal.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LoggedUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManagementBL _accountBL;

        public AccountController(IAccountManagementBL accountBL)
        {
            _accountBL = accountBL;
        }

        // GET: api/Account/Profile
        [HttpGet(RouteConfigs.Profile)]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                var userName = User.GetUserName();

                var userFromDB = await _accountBL.GetProfile(userName);

                AccountProfile result = new AccountProfile()
                {
                    UserName = userFromDB.UserName,
                    FirstName = userFromDB.FirstName,
                    LastName = userFromDB.LastName,
                    Email = userFromDB.Email,
                    DateOfBirth = userFromDB.DateOfBirth,
                    RegistrationDate = userFromDB.RegistrationDate
                };

                return Ok(result);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: api/Account/Profile
        [HttpPut(RouteConfigs.Profile)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] AccountProfileUpdate accountProfile)
        {
            try
            {
                var userName = User.GetUserName();
                ApplicationUser _user = new ApplicationUser()
                {
                    UserName = userName,
                    FirstName = accountProfile.FirstName,
                    LastName = accountProfile.LastName,
                    DateOfBirth = accountProfile.DateOfBirth
                };

                await _accountBL.UpdateProfile(_user);

                return Ok(new { Results = "Successfully Updated Profile!" });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

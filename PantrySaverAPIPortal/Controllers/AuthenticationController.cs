using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using PantrySaver.Models;
using PantrySaverAPIPortal.Consts;
using PantrySaverAPIPortal.DataTransferObjects;
using PantrySaverAPIPortal.Extensions;
using PantrySaverAPIPortal.Services.AuthenticationServices;
using PantrySaverAPIPortal.Services.EmailServices;

namespace PantrySaverAPIPortal.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAccessTokenManager _accessTokenManager;
        private readonly IEmailSender _emailSender;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
                                        SignInManager<ApplicationUser> signInManager,
                                        RoleManager<IdentityRole> roleManager,
                                        IAccessTokenManager accessTokenManager,
                                        IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _accessTokenManager = accessTokenManager;
            _emailSender = emailSender;
        }

        // POST: api/Authentication/Register
        [HttpPost(RouteConfigs.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterForm registerFrom)
        {
            if (!(await _roleManager.RoleExistsAsync("User")))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            var newId = Guid.NewGuid().ToString();
            ApplicationUser _identity = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = registerFrom.Username,
                Email = registerFrom.Email,
                FirstName = registerFrom.FirstName,
                LastName = registerFrom.LastName,
                RegistrationDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))
            };

            var result = await _userManager.CreateAsync(_identity, registerFrom.Password);

            if (result.Succeeded)
            {
                var userFromDB = await _userManager.FindByNameAsync(_identity.UserName);

                // Add default role to user("User")
                await _userManager.AddToRoleAsync(userFromDB, "User");

                // //Add Email Confirmation
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(_identity);
                var param = new Dictionary<string, string>
                {
                    {"token", token },
                    {"email", _identity.Email }
                };

                var callback = QueryHelpers.AddQueryString(registerFrom.ClientURI, param);

                await _emailSender.SendEmailAsync(_identity.Email, "Email Confirmation", EmailContent(registerFrom.ClientURI, callback));

                var roles = await _userManager.GetRolesAsync(userFromDB);
                return Created("Register successful!", new { Result = "Register successful! Please verify your email!" });
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    stringBuilder.Append(error.Description);
                }

                return BadRequest(new { Result = $"Register Fail: {stringBuilder.ToString()}" });
            }
        }

        // POST: api/Authentication/Login
        [HttpPost(RouteConfigs.Login)]
        public async Task<IActionResult> Login([FromBody] LoginForm loginForm)
        {
            var userFromDB = await _userManager.FindByNameAsync(loginForm.Username);

            if (userFromDB == null)
            {
                return BadRequest(new { Result = "Login Failed! User didn't exist in the database!" });
            }

            if (!await _userManager.IsEmailConfirmedAsync(userFromDB))
            {
                return Unauthorized((new { Result = "Email is not confirmed!" }));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(userFromDB, loginForm.Password, false);

            if (!result.Succeeded)
            {
                return BadRequest(new { Result = "Login Failed! Password didn't matched in the database!" });
            }

            if (await _userManager.GetTwoFactorEnabledAsync(userFromDB))
                return await GenerateOTPFor2StepVerification(userFromDB);

            var roles = await _userManager.GetRolesAsync(userFromDB);

            return Ok(new
            {
                Result = result,
                Username = userFromDB.UserName,
                Token = _accessTokenManager.GenerateToken(userFromDB, roles)
            });
        }

        // POST: api/Authentication/ChangePassword
        [HttpPost(RouteConfigs.ChangePassword)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm changePasswordForm)
        {
            var userName = User.GetUserName();

            var userFromDB = await _userManager.FindByNameAsync(userName);

            if (userFromDB == null)
                return BadRequest(new { Result = "Change Password Failed! User didn't exist in the database!" });

            var result = await _userManager.ChangePasswordAsync(userFromDB, changePasswordForm.OldPassword, changePasswordForm.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new { Result = "Old Password didn't matched in the database!" });

            return Ok(new { Result = "Your password has been succesfully changed!" });
        }

        [HttpGet(RouteConfigs.GenerateNewEmailConfirmation)]
        public async Task<IActionResult> GenerateNewEmailConfirmation([FromQuery] string userName, string clientURI)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userFromDB = await _userManager.FindByNameAsync(userName);

            // Add default role to user("User")
            await _userManager.AddToRoleAsync(userFromDB, "User");

            // //Add Email Confirmation
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(userFromDB);
            var param = new Dictionary<string, string>
                {
                    {"token", token },
                    {"email", userFromDB.Email }
                };

            var callback = QueryHelpers.AddQueryString(clientURI, param);

            await _emailSender.SendEmailAsync(userFromDB.Email, "Email Confirmation", EmailContent(clientURI, callback));

            return Created("Generate new email confirmation successful!", new { Result = "Generate new email confirmation successful! Please verify your email!" });
        }

        private async Task<IActionResult> GenerateOTPFor2StepVerification(ApplicationUser user)
        {
            var providers = await _userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return Unauthorized(new { Result = "Invalid 2-Step Verification Provider." });
            }

            var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
            await _emailSender.SendEmailAsync(user.Email, "Authentication Token", token);

            return Ok(new { Result = "Generated Token!" });
        }

        [HttpPost(RouteConfigs.TwoFactorAuthentication)]
        public async Task<IActionResult> TwoStepVerification([FromBody] TwoFactorAuthenticationForm twoFactorForm)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var userFromDB = await _userManager.FindByNameAsync(twoFactorForm.Email);
            if (userFromDB == null)
                return BadRequest(new { Result = "Invalid Request" });

            var validVerification = await _userManager.VerifyTwoFactorTokenAsync(userFromDB, twoFactorForm.Provider, twoFactorForm.Token);
            if (!validVerification)
                return BadRequest(new { Result = "Invalid Token Verification" });

            var roles = await _userManager.GetRolesAsync(userFromDB);

            return Ok(new
            {
                Result = validVerification,
                Username = userFromDB.UserName,
                Token = _accessTokenManager.GenerateToken(userFromDB, roles)
            });
        }

        [HttpGet(RouteConfigs.TwoFactorAuthentication)]
        public async Task<IActionResult> GetTwoStepVerification()
        {
            var userName = User.GetUserName();

            var userFromDB = await _userManager.FindByNameAsync(userName);

            if (userFromDB == null)
                return BadRequest(new { Result = "User didn't exist in the database!" });

            var result = await _userManager.GetTwoFactorEnabledAsync(userFromDB);

            return Ok(result);
        }

        [HttpPut(RouteConfigs.TwoFactorAuthentication)]
        public async Task<IActionResult> ChangeTwoStepVerification()
        {
            var userName = User.GetUserName();

            var userFromDB = await _userManager.FindByNameAsync(userName);

            if (userFromDB == null)
                return BadRequest(new { Result = "User didn't exist in the database!" });

            var result = await _userManager.GetTwoFactorEnabledAsync(userFromDB);

            if (result)
            {
                await _userManager.SetTwoFactorEnabledAsync(userFromDB, false);
                return Ok(new { Result = "Your Two Factor Authentication has been disabled!" });
            }

            await _userManager.SetTwoFactorEnabledAsync(userFromDB, true);
            return Ok(new { Result = "Your Two Factor Authentication has been enabled!" });
        }

        [HttpGet(RouteConfigs.EmailConfirmation)]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var userFromDB = await _userManager.FindByEmailAsync(email);
            if (userFromDB == null)
            {
                return BadRequest(new { Results = "Invalid Email Confirmation Request! Your email is not valid!" });
            }

            var confirmResult = await _userManager.ConfirmEmailAsync(userFromDB, token);
            if (!confirmResult.Succeeded)
            {
                return BadRequest(new { Results = "Invalid Email Confirmation Request! Your token is not valid! Please request a new one!" });
            }

            await _userManager.SetTwoFactorEnabledAsync(userFromDB, true);

            return Ok();
        }

        protected string EmailContent(string clientUri, string callback)
        {
            var emailContent = string.Format("<h2>Please verify your email at: {0}</h2><br> ", callback);
            return emailContent;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessManagement.PantryManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PantrySaver.Models;
using PantrySaverAPIPortal.Consts;
using PantrySaverAPIPortal.DataTransferObjects;
using PantrySaverAPIPortal.Extensions;

namespace PantrySaverAPIPortal.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PantryController : ControllerBase
    {
        private readonly IPantryManagementBL _pantryBL;
        private readonly UserManager<ApplicationUser> _userManager;

        public PantryController(IPantryManagementBL pantryBL,
                                UserManager<ApplicationUser> userManager)
        {
            _pantryBL = pantryBL;
            _userManager = userManager;
        }

        // GET: api/Pantry/Pantries
        [HttpGet(RouteConfigs.Pantries)]
        public async Task<IActionResult> GetPantries()
        {
            try
            {
                var userName = User.GetUserName();
                var userFromDB = await _userManager.FindByNameAsync(userName);
                if (userFromDB == null)
                    return BadRequest(new { Result = "Invalid Request! Can not find user information!" });

                return Ok(await _pantryBL.GetPantries(userFromDB.Id));
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST: api/Pantry/Pantries
        [HttpPost(RouteConfigs.Pantries)]
        public async Task<IActionResult> CreateNewPantry([FromBody] NewPantryForm newPantryForm)
        {
            try
            {
                var userName = User.GetUserName();
                Pantry _newPantry = new Pantry()
                {
                    PantryId = Guid.NewGuid().ToString(),
                    PantryName = newPantryForm.PantryName,
                    Location = newPantryForm.Location
                };

                await _pantryBL.CreateNewPantry(userName, _newPantry);

                return Ok(new { Result = "Successfully Created New Pantry!" });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // GET: api/Pantry/Pantries/5
        [HttpGet(RouteConfigs.PantryDetails)]
        public async Task<IActionResult> GetPantryDetails(string pantryId)
        {
            try
            {
                var userName = User.GetUserName();
                var userFromDB = await _userManager.FindByNameAsync(userName);
                var userID = userFromDB.Id;
                if (userFromDB == null)
                    return BadRequest(new { Result = "Invalid Request! Can not find user information!" });

                Pantry _pantry = new Pantry();

                try
                {
                    _pantry = await _pantryBL.GetPantryDetails(userID, pantryId);
                }
                catch (System.Exception e)
                {
                    return BadRequest(new { Result = "Invalid Request! You do not have the access to this pantry or the pantry is not available now!" });
                }

                if (_pantry == null)
                    return BadRequest(new { Result = "Invalid Request! Can not find pantry information!" });

                return Ok(_pantry);
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

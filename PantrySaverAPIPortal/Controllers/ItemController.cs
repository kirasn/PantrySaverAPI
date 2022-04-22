using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessManagement.ItemManagement;
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
    public class ItemController : ControllerBase
    {
        private readonly IItemManagementBL _itemBL;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemController(IItemManagementBL itemBL,
                                UserManager<ApplicationUser> userManager)
        {
            _itemBL = itemBL;
            _userManager = userManager;
        }

        // GET: api/Item/Items
        [HttpGet(RouteConfigs.Items)]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var userName = User.GetUserName();
                var userFromDB = await _userManager.FindByNameAsync(userName);

                return Ok(await _itemBL.GetItems(userFromDB.Id));
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // POST: api/Item/Items
        [HttpPost(RouteConfigs.Items)]
        public async Task<IActionResult> AddNewItem([FromBody] ItemForm itemForm)
        {
            try
            {
                var userName = User.GetUserName();
                var userFromDB = await _userManager.FindByNameAsync(userName);

                Item _item = new Item()
                {
                    ItemId = Guid.NewGuid().ToString(),
                    Name = itemForm.Name,
                    BarcodeFormats = (itemForm.BarcodeFormats != null) ? itemForm.BarcodeFormats : null,
                    Category = (itemForm.Category != null) ? itemForm.Category : null,
                    Manufacturer = (itemForm.Manufacturer != null) ? itemForm.Manufacturer : null,
                    ImageUrl = (itemForm.ImageUrl != null) ? itemForm.ImageUrl : null,
                    Description = (itemForm.Description != null) ? itemForm.Description : null,
                    IsCustom = true,
                    UserId = userFromDB.Id
                };

                await _itemBL.AddNewItem(_item);

                return Ok(new { Result = "Successfully Added A New Item!" });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // PUT: api/Item/Items/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}

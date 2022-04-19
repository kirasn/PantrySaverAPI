using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BussinessManagement.SupportManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PantrySaver.Models;
using PantrySaverAPIPortal.Consts;
using PantrySaverAPIPortal.DataTransferObjects;

namespace PantrySaverAPIPortal.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SupportController : ControllerBase
    {
        private readonly ISupportManagementBL _supportBL;

        public SupportController(ISupportManagementBL supportBL)
        {
            _supportBL = supportBL;
        }

        // POST: api/Support/Email
        [HttpPost(RouteConfigs.Email)]
        public async Task<IActionResult> PostNewEmail([FromBody] EmailSupportForm emailSupportForm)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Result = "Invalid Form Request" });

            try
            {
                EmailSupport emailSupport = new EmailSupport()
                {
                    EmailSupportId = Guid.NewGuid().ToString(),
                    EmailFrom = emailSupportForm.EmailFrom,
                    Content = emailSupportForm.Content
                };

                await _supportBL.PostNewEmail(emailSupport);

                return Ok(new { Result = "Successfully Posted A New Email Contact!" });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

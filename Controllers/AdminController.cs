using DatingApp.DTOs.Admin;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("approveVipRequest")]
        public async Task<IActionResult> ApproveVipRequestAsync([FromBody] RequestIdDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _adminService.ApproveVipRequestAsync(request.RequestId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("declineVipRequest")]
        public async Task<IActionResult> DeclineVipRequestAsync(int requestId)
        {
            await _adminService.DeclineVipRequestAsync(requestId);
            return Ok();
        }
    }
}

using AutoMapper;
using DatingApp.DTOs.Admin;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("admin")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IMapper _mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        [HttpGet("getVipRequests")]
        public async Task<IActionResult> GetVipRequestsAsync()
        {
            var requests = await _adminService.GetVipRequestsAsync();
            var result = _mapper.Map<IEnumerable<VipRequestDto>>(requests);
            return Ok(result);
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

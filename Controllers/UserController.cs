using AutoMapper;
using DatingApp.DTOs.User;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IMapper mapper, IAuthService authService)
        {
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet("getById")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user is not null)
            {
                var result = _mapper.Map<UserDto>(user);
                result.Roles = await _authService.GetUserRolesAsync(user);
                return Ok(result);
            }
            else
            {
                return BadRequest("No user with such id");
            }
        }

        [HttpDelete("delete")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(id);

            await _userService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpPut("update")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserInfoDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _userService.UpdateUserInfoAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImageAsync(string userId, IFormFile image)
        {
            if (image is null || image.Length == 0)
                return BadRequest("Incorrect image file");

            try
            {
                await _userService.UploadUserImageAsync(userId, image);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getImage")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public async Task<IActionResult> GetUserImageAsync(string userId)
        {
            var image = await _userService.GetUserImageAsync(userId);
            if (image is null)
            {
                return NotFound();
            }
            else
            {
                return File(image.Bytes, image.ContentType);
            }
        }

        [HttpDelete("deleteImage")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult DeleteUserImageAsync(string userId)
        {
            _userService.DeleteUserImage(userId);
            return Ok();
        }
    }
}

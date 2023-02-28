using AutoMapper;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Auth;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        private static void CreateUserRolesIfDontExist(RoleManager<IdentityRole> roleManager)
        {
            var userRoles = new string[2] { "Admin", "User" };
            foreach (var role in userRoles)
            {
                var roleExists = roleManager.RoleExistsAsync(role).Result;
                if (!roleExists)
                {
                    roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
        }

        public AuthController(IAuthService authService, IMapper mapper, RoleManager<IdentityRole> roleManager)
        {
            _authService = authService;
            _mapper = mapper;

            CreateUserRolesIfDontExist(roleManager);
            authService.CreateAdminUsersIfDontExist().Wait();
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(request);
            bool succeeded = await _authService.RegisterAsync(user, request.Password);

            if (succeeded)
                return Ok();
            else
                return BadRequest("Failed to register the user");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authService.LoginAsync(request.Email, request.Password);
                var userDto = _mapper.Map<UserDto>(user);
                var expireDateTime = DateTime.Now.AddMinutes(30);
                var accessToken = await _authService.GenerateAccessTokenAsync(user, expireDateTime);

                return Ok(new 
                { 
                    user = userDto,
                    accessToken = accessToken,
                    expiresAt = expireDateTime,
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("confirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool confirmed = await _authService.ConfirmEmailAsync(request.UserId);

            if (confirmed)
                return Ok();
            else
                return BadRequest();
        }
    }
}

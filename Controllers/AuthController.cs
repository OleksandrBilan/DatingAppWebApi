using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Models;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(request);
            bool succeeded = await _authService.Register(user, request.Password);

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
                var expireDateTime = DateTime.Now.AddMinutes(30);
                var accessToken = await _authService.GenerateAccessTokenAsync(user, expireDateTime);

                return Ok(new 
                { 
                    user = _mapper.Map<UserDto>(user),
                    access_token = accessToken,
                    expires_at = expireDateTime
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}

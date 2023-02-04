using AutoMapper;
using DatingApp.DB.Models;
using DatingApp.DTOs;
using DatingApp.Services.Interfaces;
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
                bool succeeded = await _authService.LoginAsync(request.Email, request.Password);
                if (succeeded)
                    return Ok();
                else
                    return BadRequest("Failed to log in");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

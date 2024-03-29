﻿using AutoMapper;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Auth;
using DatingApp.DTOs.User;
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

            authService.CreateUserRolesIfDontExistAsync().Wait();
            authService.CreateAdminUsersIfDontExistAsync().Wait();
        }

        [HttpGet("checkIfUserExists")]
        public async Task<IActionResult> CheckIfUserExistsAsync(string email)
        {
            var result = await _authService.CheckIfUserExistsAsync(email);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _mapper.Map<User>(request);
            string userId = await _authService.RegisterAsync(user, request.Password, request.QuestionnaireAnswers);

            if (userId is null)
                return StatusCode(500, "Failed to register the user");
            else
                return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] CredentialsDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _authService.LoginAsync(request.Email, request.Password);
                var expireDateTime = DateTime.Now.AddMinutes(30);
                var accessToken = await _authService.GenerateAccessTokenAsync(user, expireDateTime);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Roles = await _authService.GetUserRolesAsync(user);

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

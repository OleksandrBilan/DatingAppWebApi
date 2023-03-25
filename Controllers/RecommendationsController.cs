using AutoMapper;
using DatingApp.DTOs.Auth;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("recommendations")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
    public class RecommendationsController : Controller
    {
        private readonly IRecommendationsService _recommendationsService;
        private readonly IMapper _mapper;

        public RecommendationsController(IRecommendationsService recommendationsService, IMapper mapper)
        {
            _recommendationsService = recommendationsService;
            _mapper = mapper;
        }

        [HttpGet("getRecommendedUsers")]
        public async Task<IActionResult> GetRecommendedUsersAsync([FromQuery] FiltersDto filters)
        {
            try
            {
                var users = await _recommendationsService.GetRecommendedUsersAsync(filters);
                var result = _mapper.Map<IEnumerable<UserDto>>(users);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

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

        [HttpGet("getRecommendedUsersByFilters")]
        public async Task<IActionResult> GetRecommendedUsersByFiltersAsync(FiltersDto filters)
        {
            var users = await _recommendationsService.GetRecommendedUsersByFiltersAsync(filters);
            var result = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(result);
        }
    }
}

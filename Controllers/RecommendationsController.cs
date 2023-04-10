using AutoMapper;
using DatingApp.DB.Models.UserRelated;
using DatingApp.DTOs.Recommendations;
using DatingApp.Services.Helpers;
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

        private async Task<IActionResult> ProcessRequestAsync(RecommendationTypes recommendationType, FiltersDto filters)
        {
            try
            {
                var recommendations = await _recommendationsService.GetRecommendedUsersAsync(recommendationType, filters);
                var result = _mapper.Map<IEnumerable<RecommendedUserDto>>(recommendations);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getRecommendedUsersByFilters")]
        public async Task<IActionResult> GetRecommendedUsersByFiltersAsync([FromQuery] FiltersDto filters)
        {
            return await ProcessRequestAsync(RecommendationTypes.FiltersRecommendation, filters);
        }
    }
}

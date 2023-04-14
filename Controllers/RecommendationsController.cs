using AutoMapper;
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
        public IActionResult GetRecommendedUsers([FromQuery] FiltersDto filters)
        {
            try
            {
                var recommendations = _recommendationsService.GetRecommendedUsers(filters);
                var result = _mapper.Map<IEnumerable<RecommendedUserDto>>(recommendations);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addUserLike")]
        public async Task<IActionResult> AddUserLikeAsync([FromBody] UserLikeDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _recommendationsService.AddUserLikeAsync(request.LikingUserId, request.LikedUserId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

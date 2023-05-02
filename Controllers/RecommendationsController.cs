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

        [HttpGet("getUserLikes")]
        public async Task<IActionResult> GetUserLikesAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var recommendations = await _recommendationsService.GetUserLikesAsync(userId);
                var result = _mapper.Map<IEnumerable<RecommendedUserDto>>(recommendations);
                return Ok(result);
            }
            else
            {
                return BadRequest(userId);
            }
        }

        [HttpGet("getUserMutualLikes")]
        public async Task<IActionResult> GetUserMutualLikesAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var recommendations = await _recommendationsService.GetUserMutualLikesAsync(userId);
                var result = _mapper.Map<IEnumerable<RecommendedUserDto>>(recommendations);
                return Ok(result);
            }
            else
            {
                return BadRequest(userId);
            }
        }

        [HttpDelete("deleteUserLike")]
        public async Task<IActionResult> DeleteUserLikeAsync(int likeId)
        {
            await _recommendationsService.DeleteLikeAsync(likeId);
            return Ok();
        }

        [HttpDelete("deleteMutualLike")]
        public async Task<IActionResult> DeleteMutualLikeAsync(int likeId)
        {
            await _recommendationsService.DeleteMutualLikeAsync(likeId);
            return Ok();
        }

        [HttpPost("createChat")]
        public async Task<IActionResult> CreateChatAsync([FromBody] CreateChatDto request)
        {
            try
            {
                int chatId = await _recommendationsService.CreateChatAsync(request.MutualLikeId);
                return Ok(chatId);
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("deleteChat")]
        public async Task<IActionResult> DeleteChatAsync(int chatId)
        {
            await _recommendationsService.DeleteChatAsync(chatId);
            return Ok();
        }

        [HttpGet("getUserChats")]
        public async Task<IActionResult> GetUserChatsAsync(string userId)
        {
            var chats = await _recommendationsService.GetUserChatsAsync(userId);
            var result = _mapper.Map<IEnumerable<UsersChatDto>>(chats);
            return Ok(result);
        }

        [HttpGet("getChat")]
        public async Task<IActionResult> GetChatInfoAsync(int chatId)
        {
            var chat = await _recommendationsService.GetChatAsync(chatId);
            if (chat is null)
                return BadRequest("No chat with such id");

            var result = _mapper.Map<UsersChatDto>(chat);
            return Ok(result);
        }

        [HttpPost("setChatMessagesRead")]
        public async Task<IActionResult> SetChatMessagesReadAsync([FromBody] SetChatMessagesReadDto request)
        {
            await _recommendationsService.SetChatMessagesReadAsync(request.ChatId, request.UserId);
            return Ok();
        }
    }
}

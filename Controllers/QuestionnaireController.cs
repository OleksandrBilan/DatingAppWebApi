using AutoMapper;
using DatingApp.DTOs.Questionnaire;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("questionnaire")]
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IMapper _mapper;

        public QuestionnaireController(IQuestionnaireService questionnaireService, IMapper mapper)
        {
            _questionnaireService = questionnaireService;
            _mapper = mapper;
        }

        private async Task<IActionResult> ProcessRequestAsync(Func<Task> func)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await func();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getQuestionnaire")]
        public async Task<IActionResult> GetQuestionnaireAsync()
        {
            var questions = await _questionnaireService.GetQuestionnaireAsync();
            var result = _mapper.Map<IEnumerable<QuestionDto>>(questions);
            return Ok(result);
        }

        [HttpPost("addQuestion")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> AddQuestionAsync([FromBody] AddQuestionDto request)
        {
            return await ProcessRequestAsync(async () => 
                await _questionnaireService.AddQuestionAsync(request.Question, request.Answers));
        }

        [HttpPut("changeQuestion")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> ChangeQuestionAsync([FromBody] IdValueDto request)
        {
            return await ProcessRequestAsync(async () =>
                await _questionnaireService.ChangeQuestionAsync(request.Id, request.Value));
        }

        [HttpDelete("deleteQuestion")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteQuestionAsync(int questionId)
        {
            await _questionnaireService.DeleteQuestionAsync(questionId);
            return Ok();
        }

        [HttpPost("addAnswer")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> AddAnswerAsync([FromBody] IdValueDto request)
        {
            return await ProcessRequestAsync(async () =>
                await _questionnaireService.AddAnswerAsync(request.Id, request.Value));
        }

        [HttpPut("changeAnswer")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> ChangeAnswerAsync([FromBody] IdValueDto request)
        {
            return await ProcessRequestAsync(async () =>
                await _questionnaireService.ChangeAnswerAsync(request.Id, request.Value));
        }

        [HttpDelete("deleteAnswer")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<IActionResult> DeleteAnswerAsync(int answerId)
        {
            await _questionnaireService.DeleteAnswerAsync(answerId);
            return Ok();
        }
    }
}

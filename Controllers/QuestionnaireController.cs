using AutoMapper;
using DatingApp.DTOs.Questionnaire;
using DatingApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Route("questionnaire")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class QuestionnaireController : Controller
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IMapper _mapper;

        public QuestionnaireController(IQuestionnaireService questionnaireService, IMapper mapper)
        {
            _questionnaireService = questionnaireService;
            _mapper = mapper;
        }

        [HttpPost("addQuestion")]
        public async Task<IActionResult> AddQuestionAsync([FromBody] AddQuestionDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _questionnaireService.AddQuestionAsync(request.Question, request.Answers);
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

        [HttpPut("changeQuestion")]
        public async Task<IActionResult> ChangeQuestionAsync([FromBody] ChangeQuestionDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _questionnaireService.ChangeQuestionAsync(request.QuestionId, request.Question, request.Answers);
            return Ok();
        }

        [HttpDelete("deleteQuestion")]
        public async Task<IActionResult> DeleteQuestionAsync(int questionId)
        {
            await _questionnaireService.DeleteQuestionAsync(questionId);
            return Ok();
        }
    }
}

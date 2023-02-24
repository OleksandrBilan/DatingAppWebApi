using AutoMapper;
using DatingApp.DB.Models.Questionnaire;
using DatingApp.DTOs.Questionnaire;

namespace DatingApp.Mapping
{
    public class QuestionnaireProfile : Profile
    {
        public QuestionnaireProfile()
        {
            CreateMap<Answer, string>().ConstructUsing(a => a.Name);
            CreateMap<Question, QuestionDto>();
        }
    }
}

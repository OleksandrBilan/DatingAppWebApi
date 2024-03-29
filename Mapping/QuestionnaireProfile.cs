﻿using AutoMapper;
using DatingApp.DB.Models.Questionnaire;
using DatingApp.DTOs.Questionnaire;

namespace DatingApp.Mapping
{
    public class QuestionnaireProfile : Profile
    {
        public QuestionnaireProfile()
        {
            CreateMap<Answer, IdValueDto>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Name));

            CreateMap<Question, QuestionDto>();

            CreateMap<UserQuestionAnswer, QuestionAnswerDto>();
        }
    }
}

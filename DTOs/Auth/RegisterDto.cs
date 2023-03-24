using DatingApp.DTOs.Lookup;
using DatingApp.DTOs.Questionnaire;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public CountryDto Country { get; set; }

        [Required]
        public CityDto City { get; set; }

        [Required]
        public int SexId { get; set; }

        [Required]
        public string Description { get; set; }

        public IEnumerable<QuestionAnswerDto> QuestionnaireAnswers { get; set; }
    }
}

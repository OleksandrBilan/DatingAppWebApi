using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
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
        public int SexPreferencesId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}

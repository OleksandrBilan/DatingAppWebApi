using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class CityDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string CountryCode { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Lookup
{
    public class CountryDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}

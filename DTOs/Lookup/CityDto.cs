using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs.Lookup
{
    public class CityDto
    {
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CountryCode { get; set; }
    }
}

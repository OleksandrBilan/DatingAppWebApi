using Microsoft.AspNetCore.Identity;

namespace DatingApp.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        public Sex SexPreferences { get; set; }

        public string Description { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }
    }
}

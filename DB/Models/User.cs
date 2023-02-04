using Microsoft.AspNetCore.Identity;

namespace DatingApp.DB.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public Sex Sex { get; set; }

        public byte SexPreferences { get; set; }

        public string Description { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }
    }

    public enum Sex : byte
    {
        Male = 1, 
        Female = 2, 
        Other = 4
    }
}

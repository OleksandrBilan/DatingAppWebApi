using DatingApp.DB.Models.Locations;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.DB.Models.UserRelated
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public string Description { get; set; }

        [ForeignKey("Countries")]
        public string CountryCode { get; set; }
        public Country Country { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int SexId { get; set; }
        public Sex Sex { get; set; }
    }
}

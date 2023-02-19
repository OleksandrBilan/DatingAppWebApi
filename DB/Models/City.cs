using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.DB.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Country")]
        public string CountryCode { get; set; }
        public Country Country { get; set; }
    }
}

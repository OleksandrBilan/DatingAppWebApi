using System.ComponentModel.DataAnnotations.Schema;

namespace DatingApp.DB.Models.Locations
{
    public class City : IEquatable<City>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Country")]
        public string CountryCode { get; set; }
        public Country Country { get; set; }

        public override bool Equals(object obj)
        {
            var city = obj as City;
            return string.Equals(this.Name, city.Name, StringComparison.InvariantCultureIgnoreCase) && 
                   string.Equals(this.CountryCode, city.CountryCode, StringComparison.InvariantCultureIgnoreCase);
        }

        public bool Equals(City other)
        {
            return string.Equals(this.Name, other.Name, StringComparison.InvariantCultureIgnoreCase) &&
                   string.Equals(this.CountryCode, other.CountryCode, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

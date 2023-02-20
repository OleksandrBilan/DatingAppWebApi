namespace DatingApp.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }

        public DateTime BirthDate { get; set; }

        public int SexId { get; set; }

        public int SexPreferencesId { get; set; }

        public string Description { get; set; }

        public CountryDto Country { get; set; }

        public CityDto City { get; set; }
    }
}

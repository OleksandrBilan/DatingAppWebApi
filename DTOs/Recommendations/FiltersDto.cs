namespace DatingApp.DTOs.Recommendations
{
    public class FiltersDto
    {
        public ushort? MinAge { get; set; }

        public ushort? MaxAge { get; set; }

        public string CountryCode { get; set; }

        public int? CityId { get; set; }

        public int? PreferedSexId { get; set; }

        public bool? UseQuestionnaire { get; set; }
    }
}

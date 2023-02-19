namespace DatingApp.DB.Models
{
    public class Sex
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<User> UsersWithSuchSex { get; set; }

        public ICollection<User> UsersWithSuchSexPreferences { get; set; }
    }
}

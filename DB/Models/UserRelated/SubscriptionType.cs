namespace DatingApp.DB.Models.UserRelated
{
    public class SubscriptionType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Months { get; set; }

        public double Price { get; set; }
    }
}

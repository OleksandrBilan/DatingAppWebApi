namespace DatingApp.DB.Models.UserRelated
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public DateTime CreateDateTime { get; set; }

        public DateTime ExpireDateTime { get; set; }
    }
}

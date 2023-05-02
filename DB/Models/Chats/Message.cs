using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Chats
{
    public class Message
    {
        public int Id { get; set; }

        public string SenderId { get; set; }
        public User Sender { get; set; }

        public int ChatId { get; set; }
        public UsersChat Chat { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public int StatusId { get; set; }
        public MessageStatus Status { get; set; }
    }
}

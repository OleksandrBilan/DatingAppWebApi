using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Chats
{
    public class UsersChat
    {
        public int Id { get; set; }

        public string User1Id { get; set; }
        public User User1 { get; set; }

        public string User2Id { get; set; }
        public User User2 { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public ICollection<Message> Messages { get; set; }
    }
}

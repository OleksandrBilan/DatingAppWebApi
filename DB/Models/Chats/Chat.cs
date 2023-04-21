using DatingApp.DB.Models.UserRelated;

namespace DatingApp.DB.Models.Chats
{
    public class Chat
    {
        public int Id { get; set; }

        public string User1Id { get; set; }
        public User User1 { get; set; }

        public string User2Id { get; set; }
        public User User2 { get; set; }
    }
}

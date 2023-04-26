using DatingApp.DTOs.User;

namespace DatingApp.DTOs.Recommendations
{
    public class UsersChatDto
    {
        public int Id { get; set; }

        public UserDto User1 { get; set; }

        public UserDto User2 { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public IEnumerable<MessageDto> Messages { get; set; }
    }
}

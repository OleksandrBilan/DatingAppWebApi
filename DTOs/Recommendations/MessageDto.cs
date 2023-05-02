namespace DatingApp.DTOs.Recommendations
{
    public class MessageDto
    {
        public int Id { get; set; }

        public string SenderId { get; set; }

        public string SenderName { get; set; }

        public int ChatId { get; set; }

        public string Text { get; set; }

        public DateTime DateTime { get; set; }

        public string Status { get; set; }
    }
}

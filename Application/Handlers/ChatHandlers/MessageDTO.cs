namespace Application.ChatHandlers
{
    public class MessageDTO
    {
        public required string SenderId { get; set; }
        public required string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

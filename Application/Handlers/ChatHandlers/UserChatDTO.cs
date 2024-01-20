namespace Application.Handlers.ChatHandlers
{
    public class UserChatDTO
    {
        public int ChatId { get; set; }
        public required string ChatName { get; set; }
        public required string OtherUserId { get; set; }
    }
}

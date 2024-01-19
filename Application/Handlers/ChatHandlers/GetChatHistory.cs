using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ChatHandlers
{
    public class GetChatHistory
    {
        public class Query : IRequest<Result<ChatHistoryDTO>>
        {
            public required string User1Id { get; set; }
            public required string User2Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ChatHistoryDTO>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<ChatHistoryDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Find the chat session between the two users
                var chat = await _context.Chats
                    .FirstOrDefaultAsync(c =>
                        (c.User1Id == request.User1Id.ToString() && c.User2Id == request.User2Id.ToString()) ||
                        (c.User1Id == request.User2Id.ToString() && c.User2Id == request.User1Id.ToString()),
                        cancellationToken);

                if (chat == null)
                {
                    return Result<ChatHistoryDTO>.Failure("Chat session not found.");
                }

                // Fetch messages for the found chat session
                var messages = await _context.Messages
                    .Where(m => m.ChatId == chat.Id)
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync(cancellationToken);

                var chatHistory = new ChatHistoryDTO
                {
                    Messages = messages.Select(
                        m => new MessageDTO { 
                            SenderId = m.SenderId,
                            Content = m.Content, 
                            Timestamp = m.Timestamp 
                        })
                    .ToList()
                };

                return Result<ChatHistoryDTO>.Success(chatHistory);
            }
        }   
    }
}

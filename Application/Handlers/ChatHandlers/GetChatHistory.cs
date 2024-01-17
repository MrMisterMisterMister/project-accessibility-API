using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChatHandlers
{
    public class GetChatHistory
    {
        public class Query : IRequest<Result<ChatHistoryDto>>
        {
            public required string User1Id { get; set; }
            public required string User2Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ChatHistoryDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<ChatHistoryDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Find the chat session between the two users
                var chat = await _context.Chats
                    .FirstOrDefaultAsync(c =>
                        (c.User1Id == request.User1Id.ToString() && c.User2Id == request.User2Id.ToString()) ||
                        (c.User1Id == request.User2Id.ToString() && c.User2Id == request.User1Id.ToString()),
                        cancellationToken);

                if (chat == null)
                {
                    return Result<ChatHistoryDto>.Failure("Chat session not found.");
                }

                // Fetch messages for the found chat session
                var messages = await _context.Messages
                    .Where(m => m.ChatId == chat.Id)
                    .OrderBy(m => m.Timestamp)
                    .ToListAsync(cancellationToken);

                var chatHistory = new ChatHistoryDto
                {
                    Messages = messages.Select(
                        m => new MessageDto { 
                            SenderId = m.SenderId,
                            Content = m.Content, 
                            Timestamp = m.Timestamp 
                        })
                    .ToList()
                };

                return Result<ChatHistoryDto>.Success(chatHistory);
            }
        }

        public class ChatHistoryDto
        {
            public List<MessageDto> ?Messages { get; set; }
        }

        public class MessageDto
        {
            public string SenderId { get; set; }
            public required string Content { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}

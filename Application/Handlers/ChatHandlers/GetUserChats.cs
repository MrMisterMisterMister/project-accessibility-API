using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.ChatHandlers
{
    public class GetUserChats
    {
        public class Query : IRequest<Result<List<UserChatDTO>>>
        {
            public required string UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserChatDTO>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<UserChatDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var chats = await _context.Chats
                    .Where(c => c.User1Id == request.UserId || c.User2Id == request.UserId)
                    .Select(c => new UserChatDTO
                    {
                        ChatId = c.Id,
                        ChatName = c.User1Id == request.UserId ?
                            (c.User2Email ?? "Unknown Email") :
                            (c.User1Email ?? "Unknown Email"),
                        OtherUserId = c.User1Id == request.UserId ?
                                (c.User2Id ?? "Unknown User") :
                                (c.User1Id ?? "Unknown User")
                    })
                    .ToListAsync(cancellationToken);

                return Result<List<UserChatDTO>>.Success(chats);
            }
        }
    }
}

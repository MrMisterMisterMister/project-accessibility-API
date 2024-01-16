using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ChatHandlers
{
    public class GetUserChats
    {
        public class Query : IRequest<Result<List<UserChatDto>>>
        {
            public string UserId { get; set; }
        }

        public class UserChatDto
        {
            // Define properties here, for example:
            public int ChatId { get; set; }
            public string OtherUserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<UserChatDto>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<List<UserChatDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var chats = await _context.Chats
                    .Where(c => c.User1Id == request.UserId || c.User2Id == request.UserId)
                    .Select(c => new UserChatDto
                    {
                        ChatId = c.Id,
                        OtherUserId = c.User1Id == request.UserId ? c.User2Id : c.User1Id
                    })
                    .ToListAsync(cancellationToken);

                return Result<List<UserChatDto>>.Success(chats);
            }
        }
    }
}

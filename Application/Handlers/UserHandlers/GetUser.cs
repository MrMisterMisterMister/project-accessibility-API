using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUser
    {
        public class Query : IRequest<Result<List<User>>> { }

        public class Handler : IRequestHandler<Query, Result<List<User>>>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task<Result<List<User>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<User>>.Success(await _databaseContext.Users.ToListAsync());
            }
        }
    }
}
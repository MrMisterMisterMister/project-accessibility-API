using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUserById
    {
        public class Query : IRequest<User>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _databaseContext.Users.FindAsync(request.Id) ??
                    throw new Exception("User not found"); // will fix this later
            }
        }
    }
}
using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUserById // Handler to get user by their id
    {
        public class Query : IRequest<User> // Defines query to get user by id
        {
            public Guid Id { get; set; } // Property to hold the user's id
        }

        public class Handler : IRequestHandler<Query, User> // Handles retrieval query
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext) // Dbcontext injection
            {
                _databaseContext = databaseContext;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken) // Logic to handle retrieval proces
            {
                return await _databaseContext.Users.FindAsync(request.Id) ?? // Fetches user by their id
                    throw new Exception("User not found"); // will fix this later
            }
        }
    }
}
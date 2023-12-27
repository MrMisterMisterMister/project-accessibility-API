using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUser // Handler to retrieve users
    {
        public class Query : IRequest<List<User>> { } // Defins query to get a list of users

        public class Handler : IRequestHandler<Query, List<User>> // Handles user query retrieval
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext) // Dbcontext injcetion
            {
                _databaseContext = databaseContext;
            }

            public async Task<List<User>> Handle(Query request, CancellationToken cancellationToken) // Logic
            {
                return await _databaseContext.Users.ToListAsync(); // Fetches all users from the database asynchronously
            }
        }
    }
}
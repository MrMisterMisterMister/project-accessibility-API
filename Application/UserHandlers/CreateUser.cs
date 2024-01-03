using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class CreateUser // Handler to create a user
    {
        public class Command : IRequest // A Command to create a user
        {
            public User User { get; set; } = null!; // User information to create a user
        }

        public class Handler : IRequestHandler<Command> // Handles the creation Command
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext) // Contructor for injecting the DatabaseContext
            {
                _databaseContext = databaseContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken) // Logic to handle user creation
            {
                _databaseContext.Add(request.User); // Adds user to database context

                await _databaseContext.SaveChangesAsync(); // Saves the changes to the database
            }
        }
    }
}
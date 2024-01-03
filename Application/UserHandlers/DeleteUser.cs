using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class DeleteUser // Handler to delete a user
    {
        public class Command : IRequest // Defines Command to delete a user
        {
            public Guid Id { get; set; } // Identifier for the user to be deleted
        }

        public class Handler : IRequestHandler<Command> // Handles the Command
        {
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken) // Logic to handle user deletion
            {
                var user = await _databaseContext.Users.FindAsync(request.Id) ?? // Fetches user id from the database
                    throw new Exception("User not found"); // will fix this later

                _databaseContext.Remove(user); // Removes user from databaseContext*

                await _databaseContext.SaveChangesAsync(); // Saves changes to database
            }
        }
    }
}
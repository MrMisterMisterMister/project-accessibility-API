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
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken) // Logic to handle user deletion
            {
                var user = await _dataContext.Users.FindAsync(request.Id) ?? // Fetches user id from the database
                    throw new Exception("User not found"); // will fix this later

                _dataContext.Remove(user); // Removes user from dataContext*

                await _dataContext.SaveChangesAsync(); // Saves changes to database
            }
        }
    }
}
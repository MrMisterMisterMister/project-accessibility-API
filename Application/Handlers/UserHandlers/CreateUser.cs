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
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext) // Contructor for injecting the DataContext
            {
                _dataContext = dataContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken) // Logic to handle user creation
            {
                _dataContext.Add(request.User); // Adds user to database context

                await _dataContext.SaveChangesAsync(); // Saves the changes to the database
            }
        }
    }
}
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class EditUser
    {

        public class Command : IRequest // Handler to edit a user details
        {
            public User User { get; set; } = null!; // The user object to be edited
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;
            // Injects DatabaseContext and IMapper (so we dont have to put each user property in the Handle method)
            public Handler(DatabaseContext databaseContext, IMapper mapper) 
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken) // Logic to handle editing
            {
                var user = await _databaseContext.Users.FindAsync(request.User.Id); // Fettches user from the database using the id

                _mapper.Map(request.User, user); // Maps the properties from the request User object to the fetched User

                await _databaseContext.SaveChangesAsync(); // Saves changes to the database
            }
        }

    }
}
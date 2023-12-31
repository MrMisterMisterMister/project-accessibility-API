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
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            // Injects DataContext and IMapper (so we dont have to put each user property in the Handle method)
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken) // Logic to handle editing
            {
                var user = await _dataContext.Users.FindAsync(request.User.Id); // Fettches user from the database using the id

                _mapper.Map(request.User, user); // Maps the properties from the request User object to the fetched User

                await _dataContext.SaveChangesAsync(); // Saves changes to the database
            }
        }

    }
}
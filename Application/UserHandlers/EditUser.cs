using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class EditUser
    {

        public class Command : IRequest
        {
            public User User { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;
            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _databaseContext.Users.FindAsync(request.User.Id);

                _mapper.Map(request.User, user);

                await _databaseContext.SaveChangesAsync();
            }
        }

    }
}
using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class CreateUser
    {
        public class Command : IRequest
        {
            public User User { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;
            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                _databaseContext.Add(request.User);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
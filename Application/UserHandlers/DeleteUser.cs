using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class DeleteUser
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                var activity = await _databaseContext.Users.FindAsync(request.Id) ?? 
                    throw new Exception("User not found"); // will fix this later

                _databaseContext.Remove(activity);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
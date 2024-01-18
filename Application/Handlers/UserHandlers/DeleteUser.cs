using Application.Core;
using MediatR;
using Persistence;

namespace Application.Handlers.UserHandlers
{
    public class DeleteUser
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FindAsync(request.Id.ToString());

                if (user == null) return Result<Unit>.Failure("user not found");

                _dataContext.Remove(user);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the user");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Handlers.UserHandlers
{
    public class CreateUser
    {
        public class Command : IRequest<Result<Unit>>
        {
            public User User { get; set; } = null!;
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
                _dataContext.Add(request.User);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to create user");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
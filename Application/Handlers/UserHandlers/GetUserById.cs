using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUserById
    {
        public class Query : IRequest<Result<User>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<User>>
        {
            private readonly DataContext _dataContext;
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users.FindAsync(request.Id);

                if (user == null) return Result<User>.Failure("User not found");

                return Result<User>.Success(user);
            }
        }
    }
}
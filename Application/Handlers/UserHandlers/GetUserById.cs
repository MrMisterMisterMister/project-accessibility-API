using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.Handlers.UserHandlers
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
                var user = await _dataContext.Users.FindAsync(request.Id.ToString());

                if (user == null) return Result<User>.Failure("User not found");

                return Result<User>.Success(user);
            }
        }
    }
}

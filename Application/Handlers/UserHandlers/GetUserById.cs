using Application.Core;
using AutoMapper;
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
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<User>> Handle(Query request, CancellationToken cancellationToken)
            {
                // var user = await _dataContext.Users
                //     .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                //     .FirstOrDefaultAsync(x => x.Id == request.Id.ToString());

                var user = await _dataContext.Users.FindAsync(request.Id);

                if (user == null) return Result<User>.Failure("User not found");

                return Result<User>.Success(user);
            }
        }
    }
}
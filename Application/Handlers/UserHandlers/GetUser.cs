using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUser
    {
        public class Query : IRequest<Result<List<User>>> { }

        public class Handler : IRequestHandler<Query, Result<List<User>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<List<User>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // var users = await _dataContext.Users
                //     .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                //     .ToListAsync();

                return Result<List<User>>.Success(await _dataContext.Users.ToListAsync());
            }
        }
    }
}
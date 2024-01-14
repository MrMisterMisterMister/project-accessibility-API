using Application.Core;
using Application.Handlers.UserHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUser
    {
        public class Query : IRequest<Result<List<UserDTO>>> { }

        public class Handler : IRequestHandler<Query, Result<List<UserDTO>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<List<UserDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _dataContext.Users
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<UserDTO>>.Success(users);
            }
        }
    }
}
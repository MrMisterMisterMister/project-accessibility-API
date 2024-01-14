using Application.Core;
using Application.Handlers.UserHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserHandlers
{
    public class GetUserById
    {
        public class Query : IRequest<Result<UserDTO>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<UserDTO>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<UserDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _dataContext.Users
                    .ProjectTo<UserDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id.ToString());

                if (user == null) return Result<UserDTO>.Failure("User not found");

                return Result<UserDTO>.Success(user);
            }
        }
    }
}
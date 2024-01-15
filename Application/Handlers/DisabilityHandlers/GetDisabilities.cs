using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DisabilityHandlers
{
    public class GetDisabilities
    {
        public class Query : IRequest<Result<List<DisabilityDTO>>> { }

        public class Handler : IRequestHandler<Query, Result<List<DisabilityDTO>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<List<DisabilityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // using projection, eager loading is not needed
                // could also manually use linq but automapper is pro
                var disabilities = await _dataContext.Disabilities
                    .ProjectTo<DisabilityDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<DisabilityDTO>>.Success(disabilities);
            }
        }
    }
}

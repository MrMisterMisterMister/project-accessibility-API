using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DisabilityHandlers
{
    // Class responsible for handling the query to get a list of disabilities
    public class GetDisabilities
    {
        // Query class represents the request to get a list of disabilities
        public class Query : IRequest<Result<List<DisabilityDTO>>> { }

        // Handler class processes the GetDisabilities query
        public class Handler : IRequestHandler<Query, Result<List<DisabilityDTO>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            // Constructor initializes the handler with required dependencies
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            // Handle method executes the logic to get a list of disabilities
            public async Task<Result<List<DisabilityDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Use projection to map Disability entities to DisabilityDTOs without the need for eager loading
                // AutoMapper's ProjectTo is utilized with the configured mapping provided by ConfigurationProvider
                var disabilities = await _dataContext.Disabilities
                    .ProjectTo<DisabilityDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                // Return a success result containing the list of DisabilityDTOs
                return Result<List<DisabilityDTO>>.Success(disabilities);
            }
        }
    }
}

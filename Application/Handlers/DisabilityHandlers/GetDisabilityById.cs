using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.DisabilityHandlers
{
    // Class responsible for handling the query to get a disability by its ID
    public class GetDisabilityById
    {
        // Query class represents the request to get a disability by its ID
        public class Query : IRequest<Result<DisabilityDTO>>
        {
            // Property to hold the ID of the disability to be retrieved
            public int DisabilityId { get; set; }
        }

        // Handler class processes the GetDisabilityById query
        public class Handler : IRequestHandler<Query, Result<DisabilityDTO>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            // Constructor initializes the handler with required dependencies
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            // Handle method executes the logic to get a disability by its ID
            public async Task<Result<DisabilityDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                // Use projection to map Disability entity to DisabilityDTO without the need for eager loading
                // AutoMapper's ProjectTo is utilized with the configured mapping provided by ConfigurationProvider
                var disability = await _dataContext.Disabilities
                    .ProjectTo<DisabilityDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.DisabilityId);

                // If the disability is not found, return a failure result with a message
                if (disability == null)
                    return Result<DisabilityDTO>.Failure("DisabilityNotFound", "The disability could not be found.");

                // Return a success result containing the DisabilityDTO
                return Result<DisabilityDTO>.Success(disability);
            }
        }
    }
}

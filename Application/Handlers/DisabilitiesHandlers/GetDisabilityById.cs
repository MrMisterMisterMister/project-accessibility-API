using Application.Core;
using Application.Handlers.ResearchesHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class GetDisabilityById
    {
        public class Query : IRequest<Result<DisabilityDTO>>
        {
            public int DisabilityId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<DisabilityDTO>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<DisabilityDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                // maps data into entities
                // could also be eagerloaded (also loads unneeded data)
                //  or using linq queris
                // but it's easier with automapper
                var disability = await _dataContext.Disabilities
                    .ProjectTo<DisabilityDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.DisabilityId);

                if (disability == null) return Result<DisabilityDTO>.Failure("DisabilityNotFound", "The disability could not be found.");

                return Result<DisabilityDTO>.Success(disability);
            }
        }
    }
}

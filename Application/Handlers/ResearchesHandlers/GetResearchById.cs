using Application.Core;
using Application.Handlers.ResearchesHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class GetResearchById
    {
        public class Query : IRequest<Result<ResearchDTO>>
        {
            public int ResearchId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ResearchDTO>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<ResearchDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                // maps data into entities
                // could also be eagerloaded (also loads unneeded data)
                //  or using linq queris
                // but it's easier with automapper
                var research = await _dataContext.Researches
                    .ProjectTo<ResearchDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.ResearchId);

                if (research == null) return Result<ResearchDTO>.Failure("ResearchNotFound", "The research could not be found.");

                return Result<ResearchDTO>.Success(research);
            }
        }
    }
}

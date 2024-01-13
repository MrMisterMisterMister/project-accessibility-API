using Application.Core;
using Application.Handlers.ResearchesHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.ResearchesHandlers
{
    public class GetResearch
    {
        public class Query : IRequest<Result<List<ResearchDTO>>> { }

        public class Handler : IRequestHandler<Query, Result<List<ResearchDTO>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<List<ResearchDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                // using projection, eager loading is not needed
                // could also manually use linq but automapper is pro
                var researches = await _dataContext.Researches
                    .ProjectTo<ResearchDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<ResearchDTO>>.Success(researches);
            }
        }
    }
}

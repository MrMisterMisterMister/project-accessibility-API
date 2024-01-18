using Application.Core;
using Application.Handlers.PanelMemberHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class GetPanelMember
    {
        public class Query : IRequest<Result<List<PanelMemberDTO>>> { }

        public class Handler : IRequestHandler<Query, Result<List<PanelMemberDTO>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<List<PanelMemberDTO>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var panelMembers = await _dataContext.PanelMembers
                    .ProjectTo<PanelMemberDTO>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<PanelMemberDTO>>.Success(panelMembers);
            }
        }
    }
}
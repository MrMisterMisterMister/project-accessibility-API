using Application.Core;
using Application.Handlers.PanelMemberHandlers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class GetPanelMemberById
    {
        public class Query : IRequest<Result<PanelMemberDTO>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<PanelMemberDTO>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<PanelMemberDTO>> Handle(Query request, CancellationToken cancellationToken)
            {

                // maps data into entities
                // could also be eagerloaded (also loads unneeded data)
                //  or using linq queris
                // but it's easier with automapper
                var panelMember = await _dataContext.PanelMembers
                    .ProjectTo<PanelMemberDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id.ToString());

                if (panelMember == null) return Result<PanelMemberDTO>.Failure("PanelMemberNotFound", "Panel member could not be found.");

                return Result<PanelMemberDTO>.Success(panelMember);


            }
        }
    }
}
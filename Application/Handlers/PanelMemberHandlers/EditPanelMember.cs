using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class EditPanelMember
    {

        public class Command : IRequest
        {
            public PanelMember PanelMember { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _dataContext.PanelMembers.FindAsync(request.PanelMember.Id);
                _mapper.Map(request.PanelMember, company);

                await _dataContext.SaveChangesAsync();
            }
        }

    }
}
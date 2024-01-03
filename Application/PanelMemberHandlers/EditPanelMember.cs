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
            private readonly DatabaseContext _databaseContext;
            private readonly IMapper _mapper;
            public Handler(DatabaseContext databaseContext, IMapper mapper)
            {
                _mapper = mapper;
                _databaseContext = databaseContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _databaseContext.PanelMembers.FindAsync(request.PanelMember.Id);
                _mapper.Map(request.PanelMember, company);

                await _databaseContext.SaveChangesAsync();
            }
        }

    }
}
using Application.Core;
using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class EditPanelMember
    {

        public class Command : IRequest<Result<Unit>>
        {
            public PanelMember PanelMember { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var panelMember = await _dataContext.PanelMembers.FindAsync(request.PanelMember.Id);

                // TODO researches

                if (panelMember == null) return Result<Unit>.Failure("Panel member not found");

                request.PanelMember.UserName = panelMember.UserName;
                request.PanelMember.NormalizedUserName = panelMember.NormalizedUserName;
                request.PanelMember.Email = panelMember.Email;
                request.PanelMember.NormalizedEmail = panelMember.NormalizedEmail;
                request.PanelMember.PasswordHash = panelMember.PasswordHash;

                _mapper.Map(request.PanelMember, panelMember);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to update panel member");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
using Application.Core;
using MediatR;
using Persistence;

namespace Application.PanelMemberHandlers
{
    public class DeletePanelMember
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var panelMember = await _dataContext.PanelMembers.FindAsync(request.Id.ToString());

                if (panelMember == null) return Result<Unit>.Failure("Panel member not found");

                _dataContext.Remove(panelMember);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete the panel member");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
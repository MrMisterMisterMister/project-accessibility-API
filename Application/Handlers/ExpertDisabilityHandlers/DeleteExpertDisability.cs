using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Handlers.ExpertDisabilityHandlers
{
    public class DeleteExpertDisability
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int DisabilityId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;
            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var disability = await _dataContext.Disabilities
                    .Include(x => x.Experts)
                    .ThenInclude(x => x.PanelMember)
                    .FirstOrDefaultAsync(x => x.Id == request.DisabilityId);

                if (disability == null)
                    return Result<Unit>.Failure("DisabilityNotFound", "The disabiltiy could not be found.");

                var expert = await _dataContext.PanelMembers.FirstOrDefaultAsync(x =>
                    x.Email == _userAccessor.GetEmail()
                );

                if (expert == null)
                    return Result<Unit>.Failure("PanelMemberNotFound", "The panel member could not be found.");

                var hasDisability = disability.Experts.FirstOrDefault(x => x.PanelMember == expert);

                if (hasDisability == null)
                    return Result<Unit>.Failure("ExpertDoesNotHaveDisability", "The expert does not have this disability.");

                disability.Experts.Remove(hasDisability);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result)
                    return Result<Unit>.Failure("defaultMessage");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
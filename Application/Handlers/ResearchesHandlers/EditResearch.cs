using Application.Core;
using MediatR;
using AutoMapper;
using Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.ResearchesHandlers
{
    public class EditResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var research = await _dataContext.Researches
                .Include(x => x.Organizer)
                .Include(r => r.Participants)
                    .ThenInclude(p => p.PanelMember)
                .FirstOrDefaultAsync(x => x.Id == request.Research.Id);

                if (research == null)
                    return Result<Unit>.Failure("ResearchNotFound", "The research could not be found.");

                var organizer = await _dataContext.Companies
                    .FirstOrDefaultAsync(x => x.Email == _userAccessor.GetEmail());

                if (organizer == null)
                    return Result<Unit>.Failure("OrganizerNotFound", "The organizer could not be found.");

                if (organizer.Id != research.Organizer!.Id)
                    return Result<Unit>.Failure("OrganizerNotTheSame", "The organizer needs to be the same.");

                // Set the Organizer and OrganizerId
                request.Research.Organizer = organizer;
                request.Research.OrganizerId = organizer.Id;

                _mapper.Map(request.Research, research);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("ResearchedFailedUpdate", "The research could not be updated.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

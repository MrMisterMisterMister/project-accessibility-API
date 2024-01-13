using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ParticipantsHandlers
{
    public class AddResearchParticipant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int ResearchId { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, ILogger<Handler> logger, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Bezig met toevoegen van deelnemer...");
                try
                {
                    // eager load attendees
                    var onderzoek = await _dataContext.Researches
                        .Include(a => a.Organizer)
                        .Include(a => a.Participants)
                        .ThenInclude(u => u.PanelMemberId)
                        .FirstOrDefaultAsync(x => x.Id == request.ResearchId);
                    // can also use singleordefaultasync, difference is that returns
                    // an exception and this returns null

                    if (onderzoek == null)
                    {
                        return Result<Unit>.Failure("Onderzoek bestaat niet.");
                    }

                    var deelnemer = await _dataContext.Users.FirstOrDefaultAsync(x =>
                    x.Email == _userAccessor.GetEmail());

                    if (deelnemer == null)
                    {
                        return Result<Unit>.Failure("Deelnemer bestaat niet.");
                    }

                    // Check of de deelnemer al voorkomt in het onderzoek om dubbele toevoegingen te voorkomen
                    if (onderzoek.Participants?.Any(dp => dp.PanelMember.Id == deelnemer.Id) == true)
                    {
                        return Result<Unit>.Failure($"Deelnemer {deelnemer.UserName} komt al voor in het onderzoek");
                    }

                    // Resultaat is true als er changes zijn opgeslagen en false als er geen zijn opgeslagen.
                    bool resultaat = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

                    if (!resultaat)
                    {
                        return Result<Unit>.Failure($"Probleem opgetreden bij het toevoegen van de deelnemer. Id: {deelnemer.Id}");
                    }

                    _logger.LogInformation($"Succesvol deelnemer {deelnemer.UserName} toegevoegd aan onderzoek");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fout opgetreden bij ophalen van onderzoek");
                    return Result<Unit>.Failure("Fout opgetreden bij ophalen van onderzoek");
                }
            }
        }
    }
}

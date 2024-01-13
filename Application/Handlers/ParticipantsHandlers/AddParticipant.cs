using Application.Core;
using Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ParticipantsHandlers
{
    public class AddParticipant
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
            public PanelMember Participant { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly ILogger<Handler> _logger;

            public Handler(DataContext dataContext, ILogger<Handler> logger)
            {
                _dataContext = dataContext;
                _logger = logger;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Bezig met toevoegen van deelnemer...");
                try
                {
                    var onderzoek = await _dataContext.Researches.FindAsync(request.Research);

                    if (onderzoek == null)
                    {
                        return Result<Unit>.Failure("Onderzoek bestaat niet.");
                    }

                    var deelnemer = await _dataContext.PanelMembers.FindAsync(request.Participant);

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
                        return Result<Unit>.Failure($"Probleem opgetreden bij het toevoegen van de deelnemer. Id: {request.Participant.Id}");
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

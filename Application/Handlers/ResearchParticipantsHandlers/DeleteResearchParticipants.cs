using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Application.ParticipantsHandlers
{
    public class DeleteResearchParticipants
    {
        public class Command : IRequest<Result<Unit>>
        {
            public int Research { get; set; }
            public string Participant { get; set; } = null!;
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
        _logger.LogInformation("Bezig met ophalen van onderzoek...");
        try
        {
            var onderzoek = await _dataContext.Researches.Include(r => r.Participants).FirstOrDefaultAsync(r => r.Id == request.Research);

            if (onderzoek == null)
            {
                return Result<Unit>.Failure("Onderzoek niet gevonden");
            }

            if (onderzoek.Participants == null)
            {
                return Result<Unit>.Failure("Onderzoek bevat geen deelnemers.");
            }

            _logger.LogInformation($"Number of participants in research {request.Research}: {onderzoek.Participants.Count}");

               var deelnemerTeVewijderen = onderzoek.Participants
             .FirstOrDefault(p => p.PanelMemberId == request.Participant);

            if (deelnemerTeVewijderen == null)
            {
                return Result<Unit>.Failure($"Deelnemer: {request.Participant} die verwijderd moet worden komt niet voor in onderzoek {request.Research} of bestaat niet.");
            }

            // Als de deelnemer vooorkomt in het onderzoek wordt hij verwijderd
            onderzoek.Participants.Remove(deelnemerTeVewijderen);
            var resultaat = await _dataContext.SaveChangesAsync(cancellationToken) > 0;

            if (!resultaat)
            {
                _logger.LogError("Probleem opgetreden bij het verwijderen van de geselecteerde deelnemer. Opslaan in de database mislukt.");
                return Result<Unit>.Failure("Probleem opgetreden bij het verwijderen van de geselecteerde deelnemer.");
            }

            _logger.LogInformation($"Succesvol deelnemer {deelnemerTeVewijderen.PanelMemberId} verwijderd van onderzoek {request.Research}.");
            return Result<Unit>.Success(Unit.Value);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Er is een fout opgetreden bij het verwijderen van de deelnemer");
            return Result<Unit>.Failure("Er is een fout opgetreden tijdens het verwijderen van de deelnemer.");
        }
    }
}
    }
}
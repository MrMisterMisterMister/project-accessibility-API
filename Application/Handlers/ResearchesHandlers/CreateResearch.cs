using Application.Core;
using Domain;
using MediatR;
using Persistence;
using Microsoft.Extensions.Logging;

namespace Application.ResearchesHandlers
{
    public class CreateResearch
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Research Research { get; set; } = null!;
            public Company Organizer { get; set; } = null!;
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
                try
                {
                    if (request.Research == null)
                    {
                        return Result<Unit>.Failure("Ongeldig verzoek, het onderzoeksobject mag niet leeg zijn.");
                    }

                    if (request.Research.Organizer == null)
                    {
                        return Result<Unit>.Failure("Ongeldig verzoek, de organisator mag niet leeg zijn.");
                    }

                    if (request.Research.Organizer.Id == null && !string.IsNullOrEmpty(request.Organizer.Id))
                    {
                        request.Research.Organizer.Id = request.Organizer.Id;
                    }

                    _dataContext.Add(request.Research);

                    var result = await _dataContext.SaveChangesAsync(cancellationToken) > 0;
                    Console.WriteLine($"Onderzoek aanmaken voltooid: {result}");

                    if (!result)
                    {
                        return Result<Unit>.Failure("Mislukt om onderzoek aan te maken");
                    }
                    _logger.LogInformation($"Succesvol onderzoek aangemaakt met titel: {request.Research.Title} en id: {request.Research.Id}");
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure($"Er is een fout opgetreden: {ex.Message}");
                }
            }
        }
    }
}

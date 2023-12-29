using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class DeleteCompany
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DatabaseContext _databaseContext;

            public Handler(DatabaseContext databaseContext)
            {
                _databaseContext = databaseContext;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _databaseContext.Companies.FindAsync(request.Id) ?? 
                    throw new Exception("Company not found"); 

                _databaseContext.Remove(company);

                await _databaseContext.SaveChangesAsync();
            }
        }
    }
}
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
            private readonly DataContext _dateContext;

            public Handler(DataContext dataContext)
            {
                _dateContext = dataContext;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _dateContext.Companies.FindAsync(request.Id) ?? 
                    throw new Exception("Company not found"); 

                _dateContext.Remove(company);

                await _dateContext.SaveChangesAsync();
            }
        }
    }
}
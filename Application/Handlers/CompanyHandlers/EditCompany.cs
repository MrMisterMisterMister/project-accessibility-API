using AutoMapper;
using Domain;
using MediatR;
using Persistence;

namespace Application.CompanyHandlers
{
    public class EditCompany
    {

        public class Command : IRequest
        {
            public Company Company { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            public Handler(DataContext dataContext, IMapper mapper)
            {
                _mapper = mapper;
                _dataContext = dataContext;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var company = await _dataContext.Companies.FindAsync(request.Company.Id);
                _mapper.Map(request.Company, company);

                await _dataContext.SaveChangesAsync();
            }
        }

    }
}
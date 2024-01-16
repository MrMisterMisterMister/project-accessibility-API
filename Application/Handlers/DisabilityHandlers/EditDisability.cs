using Application.Core;
using MediatR;
using AutoMapper;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Models.Disabilities;

namespace Application.DisabilityHandlers
{
    public class EditDisability
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Disability Disability { get; set; } = null!;
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;


            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var disability = await _dataContext.Disabilities
                    .Include(x => x.Experts)
                    .ThenInclude(x => x.PanelMember)
                    .FirstOrDefaultAsync(x => x.Id == request.Disability.Id);

                if (disability == null)
                    return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                foreach (var expert in disability.Experts)
                    request.Disability.Experts.Add(expert);

                _mapper.Map(request.Disability, disability);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("DisabilityFailedUpdate", "The disability could not be updated.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

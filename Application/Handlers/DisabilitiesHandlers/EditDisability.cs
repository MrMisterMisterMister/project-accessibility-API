using Application.Core;
using MediatR;
using AutoMapper;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Models.Disabilities;

namespace Application.ResearchesHandlers
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
                // TODO might need to include experts as well
                // just in case there's a bug where the list is emptied
                var disability = await _dataContext.Disabilities
                .FirstOrDefaultAsync(x => x.Id == request.Disability.Id);

                if (disability == null)
                    return Result<Unit>.Failure("DisabilityNotFound", "The disability could not be found.");

                _mapper.Map(request.Disability, disability);

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("DisabilityFailedUpdate", "The disability could not be updated.");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}

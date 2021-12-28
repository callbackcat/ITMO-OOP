using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Common.Exceptions;
using Reports.Application.Interfaces;

namespace Reports.Application.Tasks.Queries.GetTaskDetails
{
    public class GetTaskDetailsQueryHandler : IRequestHandler<GetTaskDetailsQuery, TaskDetailsVm>
    {
        private readonly IReportsDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTaskDetailsQueryHandler(IReportsDbContext dbContext, IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<TaskDetailsVm> Handle(GetTaskDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Tasks
                .FirstOrDefaultAsync(task => task.Id == request.Id, cancellationToken);

            if (entity is null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Task), request.Id);
            }

            return _mapper.Map<TaskDetailsVm>(entity);
        }
    }
}
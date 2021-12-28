using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Interfaces;

namespace Reports.Application.Tasks.Queries.GetTaskList
{
    public class GetTaskListQueryHandler : IRequestHandler<GetTaskListQuery, TaskListVm>
    {
        private readonly IReportsDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetTaskListQueryHandler(IReportsDbContext dbContext, IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<TaskListVm> Handle(GetTaskListQuery request, CancellationToken cancellationToken)
        {
            var reportsQuery = await _dbContext.Tasks
                .Where(task => task.UserId == request.UserId)
                .ProjectTo<TaskLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new TaskListVm { Tasks = reportsQuery };
        }
    }
}
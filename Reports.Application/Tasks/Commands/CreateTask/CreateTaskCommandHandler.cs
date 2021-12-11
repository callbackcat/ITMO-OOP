using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Reports.Application.Interfaces;
using Task = Reports.Domain.Task;

namespace Reports.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly IReportsDbContext _dbContext;

        public CreateTaskCommandHandler(IReportsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = new Task
            {
                UserId = request.UserId,
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                CreationTime = DateTime.Now,
                EditTime = null,
                Status = TaskStatus.Created
            };

            await _dbContext.Tasks.AddAsync(task, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Common.Exceptions;
using Reports.Application.Interfaces;

namespace Reports.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand>
    {
        private readonly IReportsDbContext _dbContext;

        public UpdateTaskCommandHandler(IReportsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Tasks
                .FirstOrDefaultAsync(task => task.Id == request.Id, cancellationToken);

            if (entity is null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Task), request.Id);
            }

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.UserId = request.UserId;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
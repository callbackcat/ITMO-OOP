using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Common.Exceptions;
using Reports.Application.Interfaces;
using Reports.Domain.ReportEntities;

namespace Reports.Application.Reports.Commands.UpdateReport
{
    public class UpdateReportCommandHandler : IRequestHandler<UpdateReportCommand>
    {
        private readonly IReportsDbContext _dbContext;

        public UpdateReportCommandHandler(IReportsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Unit> Handle(UpdateReportCommand request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Reports
                .FirstOrDefaultAsync(report => report.Id == request.Id, cancellationToken);

            if (entity is null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Report), request.Id);
            }

            entity.Title = request.Title;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
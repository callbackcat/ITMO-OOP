using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Reports.Application.Interfaces;
using Reports.Domain.Enums;
using Reports.Domain.ReportEntities;

namespace Reports.Application.Reports.Commands.CreateReport
{
    public class CreateReportCommandHandler : IRequestHandler<CreateReportCommand,  Guid>
    {
        private readonly IReportsDbContext _dbContext;

        public CreateReportCommandHandler(IReportsDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task<Guid> Handle(CreateReportCommand request, CancellationToken cancellationToken)
        {
            var report = new Report
            {
                UserId = request.UserId,
                Id = Guid.NewGuid(),
                Title = request.Title,
                CreationTime = DateTime.Now,
             //   State = ReportState.Draft
            };

            await _dbContext.Reports.AddAsync(report, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return report.Id;
        }
    }
}
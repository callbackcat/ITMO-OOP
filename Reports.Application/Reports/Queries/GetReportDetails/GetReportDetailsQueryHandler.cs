using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Common.Exceptions;
using Reports.Application.Interfaces;
using Reports.Domain.ReportEntities;

namespace Reports.Application.Reports.Queries.GetReportDetails
{
    public class GetReportDetailsQueryHandler : IRequestHandler<GetReportDetailsQuery, ReportDetailsVm>
    {
        private readonly IReportsDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetReportDetailsQueryHandler(IReportsDbContext dbContext, IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<ReportDetailsVm> Handle(GetReportDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Reports
                .FirstOrDefaultAsync(report => report.Id == request.Id, cancellationToken);

            if (entity is null || entity.UserId != request.UserId)
            {
                throw new NotFoundException(nameof(Report), request.Id);
            }

            return _mapper.Map<ReportDetailsVm>(entity);
        }
    }
}
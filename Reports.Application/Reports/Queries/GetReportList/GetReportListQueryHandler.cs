using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reports.Application.Interfaces;

namespace Reports.Application.Reports.Queries.GetReportList
{
    public class GetReportListQueryHandler : IRequestHandler<GetReportListQuery, ReportListVm>
    {
        private readonly IReportsDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetReportListQueryHandler(IReportsDbContext dbContext, IMapper mapper) =>
            (_dbContext, _mapper) = (dbContext, mapper);

        public async Task<ReportListVm> Handle(GetReportListQuery request, CancellationToken cancellationToken)
        {
            var reportsQuery = await _dbContext.Reports
                .Where(report => report.UserId == request.UserId)
                .ProjectTo<ReportLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return new ReportListVm { Reports = reportsQuery };
        }
    }
}
using System;
using MediatR;

namespace Reports.Application.Reports.Queries.GetReportList
{
    public class GetReportListQuery : IRequest<ReportListVm>
    {
        public Guid UserId { get; set; }
    }
}
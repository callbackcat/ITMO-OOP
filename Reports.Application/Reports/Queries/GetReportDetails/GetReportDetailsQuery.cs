using System;
using MediatR;

namespace Reports.Application.Reports.Queries.GetReportDetails
{
    public class GetReportDetailsQuery : IRequest<ReportDetailsVm>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
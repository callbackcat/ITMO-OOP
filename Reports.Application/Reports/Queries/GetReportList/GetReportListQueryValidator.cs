using System;
using FluentValidation;

namespace Reports.Application.Reports.Queries.GetReportList
{
    public class GetReportListQueryValidator : AbstractValidator<GetReportListQuery>
    {
        public GetReportListQueryValidator()
        {
            RuleFor(report => report.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
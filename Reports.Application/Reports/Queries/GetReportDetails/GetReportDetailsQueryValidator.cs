using System;
using FluentValidation;

namespace Reports.Application.Reports.Queries.GetReportDetails
{
    public class GetReportDetailsQueryValidator : AbstractValidator<GetReportDetailsQuery>
    {
        public GetReportDetailsQueryValidator()
        {
            RuleFor(report => report.UserId)
                .NotEqual(Guid.Empty);

            RuleFor(report => report.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
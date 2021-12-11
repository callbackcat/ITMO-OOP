using System;
using FluentValidation;

namespace Reports.Application.Reports.Commands.UpdateReport
{
    public class UpdateReportCommandValidator : AbstractValidator<UpdateReportCommand>
    {
        public UpdateReportCommandValidator()
        {
            RuleFor(report => report.UserId)
                .NotEqual(Guid.Empty);

            RuleFor(report => report.Id)
                .NotEqual(Guid.Empty);

            RuleFor(report => report.Title)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
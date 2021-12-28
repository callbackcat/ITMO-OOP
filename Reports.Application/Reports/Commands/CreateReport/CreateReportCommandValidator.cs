using System;
using FluentValidation;

namespace Reports.Application.Reports.Commands.CreateReport
{
    public class CreateReportCommandValidator : AbstractValidator<CreateReportCommand>
    {
        public CreateReportCommandValidator()
        {
            RuleFor(report => report.Title)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(report => report.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
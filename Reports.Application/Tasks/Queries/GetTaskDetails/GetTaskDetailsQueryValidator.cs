using System;
using FluentValidation;

namespace Reports.Application.Tasks.Queries.GetTaskDetails
{
    public class GetTaskDetailsQueryValidator : AbstractValidator<GetTaskDetailsQuery>
    {
        public GetTaskDetailsQueryValidator()
        {
            RuleFor(task => task.UserId)
                .NotEqual(Guid.Empty);

            RuleFor(task => task.Id)
                .NotEqual(Guid.Empty);
        }
    }
}
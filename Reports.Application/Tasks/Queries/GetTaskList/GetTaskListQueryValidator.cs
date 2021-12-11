using System;
using FluentValidation;

namespace Reports.Application.Tasks.Queries.GetTaskList
{
    public class GetTaskListQueryValidator : AbstractValidator<GetTaskListQuery>
    {
        public GetTaskListQueryValidator()
        {
            RuleFor(task => task.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
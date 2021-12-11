using System;
using FluentValidation;

namespace Reports.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandValidator :  AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(task => task.Title)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(task => task.Description)
                .NotEmpty()
                .MaximumLength(250);

            RuleFor(task => task.UserId)
                .NotEqual(Guid.Empty);
        }
    }
}
using System;
using FluentValidation;

namespace Reports.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator()
        {
            RuleFor(task => task.UserId)
                .NotEqual(Guid.Empty);

            RuleFor(task => task.Id)
                .NotEqual(Guid.Empty);

            RuleFor(task => task.Title)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(task => task.Description)
                .NotEmpty()
                .MaximumLength(250);
        }
    }
}
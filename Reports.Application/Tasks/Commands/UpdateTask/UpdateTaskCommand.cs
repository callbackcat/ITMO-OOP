using System;
using MediatR;

namespace Reports.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
using System;
using System.Threading.Tasks;
using MediatR;

namespace Reports.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
    }
}
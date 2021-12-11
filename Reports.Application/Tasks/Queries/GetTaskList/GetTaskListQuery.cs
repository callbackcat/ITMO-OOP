using System;
using MediatR;

namespace Reports.Application.Tasks.Queries.GetTaskList
{
    public class GetTaskListQuery : IRequest<TaskListVm>
    {
        public Guid UserId { get; set; }
    }
}
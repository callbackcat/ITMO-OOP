using System;
using MediatR;

namespace Reports.Application.Tasks.Queries.GetTaskDetails
{
    public class GetTaskDetailsQuery : IRequest<TaskDetailsVm>
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
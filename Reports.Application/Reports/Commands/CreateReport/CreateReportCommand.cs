using System;
using MediatR;

namespace Reports.Application.Reports.Commands.CreateReport
{
    public class CreateReportCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string Title { get; set; }
    }
}
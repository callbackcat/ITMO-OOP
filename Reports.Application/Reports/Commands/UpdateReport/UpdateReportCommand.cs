using System;
using System.Collections.Generic;
using MediatR;
using Reports.Domain;
using Reports.Domain.Enums;

namespace Reports.Application.Reports.Commands.UpdateReport
{
    public class UpdateReportCommand : IRequest
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ReportState State { get; set; }
        public IList<Task> Tasks { get; set; }
    }
}
using System;
using System.Collections.Generic;
using Reports.Domain.Enums;

namespace Reports.Domain.ReportEntities
{
    public class Report
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public ReportState State { get; set; }
        public IList<Task> Tasks { get; set; }
    }
}
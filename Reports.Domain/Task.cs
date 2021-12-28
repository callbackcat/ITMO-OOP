using System;
using System.Threading.Tasks;

namespace Reports.Domain
{
    public class Task
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? EditTime { get; set; }
        public TaskStatus Status { get; set; }
    }
}
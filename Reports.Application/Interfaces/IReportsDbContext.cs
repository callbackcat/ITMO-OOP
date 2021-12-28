using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.Domain.ReportEntities;
using Task = Reports.Domain.Task;

namespace Reports.Application.Interfaces
{
    public interface IReportsDbContext
    {
        DbSet<Report> Reports { get; set; }
        DbSet<Task> Tasks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
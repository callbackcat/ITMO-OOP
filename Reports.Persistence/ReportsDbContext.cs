using Microsoft.EntityFrameworkCore;
using Reports.Application.Interfaces;
using Reports.Domain;
using Reports.Domain.ReportEntities;
using Reports.Persistence.EntityTypeConfigurations;

namespace Reports.Persistence
{
    public class ReportsDbContext : DbContext, IReportsDbContext
    {
        public DbSet<Report> Reports { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public ReportsDbContext(DbContextOptions<ReportsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
using System.Threading.Tasks;
using Reports.Auth.Core.Repositories;

namespace Reports.Auth.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AuthDbContext _context;

        public UnitOfWork(AuthDbContext context) =>
            _context = context;

        public async Task CompleteAsync() =>
            await _context.SaveChangesAsync();
    }
}
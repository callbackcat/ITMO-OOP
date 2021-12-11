using System.Threading.Tasks;

namespace Reports.Auth.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
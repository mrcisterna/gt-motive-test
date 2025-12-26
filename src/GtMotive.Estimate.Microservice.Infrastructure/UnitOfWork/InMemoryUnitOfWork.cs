using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.Infrastructure.UnitOfWork
{
    /// <summary>
    /// Unit of work implementation for in-memory storage.
    /// </summary>
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Applies all database changes.
        /// </summary>
        /// <returns>Number of affected rows.</returns>
        public Task<int> Save()
        {
            // In-memory storage is already persistent
            return Task.FromResult(1);
        }
    }
}

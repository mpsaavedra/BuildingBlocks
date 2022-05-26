using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Orun.BuildingBlocks.Domain
{
    /// <summary>
    /// Unit of Work to operate execute a command within the database
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Transaction related value
        /// </summary>
        IDbContextTransaction Transaction { get; }

        /// <summary>
        /// Open a new database connection
        /// </summary>
        void OpenTransaction();

        /// <summary>
        /// Opens a new async database connection
        /// </summary>
        /// <returns>Task.</returns>
        Task OpenTransactionAsync();

        /// <summary>
        /// Save operation data into the database
        /// </summary>
        /// <param name="useChangeTracker">if true it will use Change tracking mechanism.</param>
        void Save(bool useChangeTracker = true);

        /// <summary>
        /// Async save operation data into the database.
        /// </summary>
        /// <param name="useChangeTracker">if true it will use Change tracking mechanism</param>
        /// <returns>Task.</returns>
        Task SaveAsync(bool useChangeTracker = true);

        /// <summary>
        /// Close active transaction
        /// </summary>
        void CloseTransaction();

        /// <summary>
        /// Asynchronously close active transaction
        /// </summary>
        /// <returns></returns>
        Task CloseTransactionAsync();
    }
}
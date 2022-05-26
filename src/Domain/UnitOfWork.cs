using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Orun.Domain;
using Orun.Extensions;

namespace Orun.BuildingBlocks.Domain
{
    /// <summary>
    /// <inheritdoc cref="IUnitOfWork"/>. It will use the application DbContext to execute
    /// the database operations.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public partial class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        /// <inheritdoc cref="IUnitOfWork.Transaction"/>
        public IDbContextTransaction Transaction { get; private set; }

        private TDbContext Context { get; }

        public UnitOfWork(TDbContext context)
        {
            Context = context.IsNullOrEmpty($"{nameof(context)} could not be null or empty");
        }

        /// <inheritdoc cref="IUnitOfWork.OpenTransaction()"/>
        public virtual async void OpenTransaction()
        {
            if (!Is.NullOrEmpty(Transaction))
            {
                Transaction.Dispose();
            }

            Transaction = await Context.Database.BeginTransactionAsync();
        }

        /// <inheritdoc cref="IUnitOfWork.OpenTransactionAsync()"/>
        public virtual async Task OpenTransactionAsync()
        {
            if (!Is.NullOrEmpty(Transaction))
            {
                await Task.Run(async () => Transaction.Dispose());
            }

            Transaction = await Context.Database.BeginTransactionAsync();
        }

        /// <inheritdoc cref="IUnitOfWork.Save(bool)"/>
        public virtual void Save(bool useChangeTracker = true)
        {
            try
            {
                // Context.UseChangeTracker(useChangeTracker).SaveChanges();
                Context.SaveChanges();

                Transaction?.Commit();
            }
            catch (Exception exception)
            {
                CloseTransaction();
                ThrowSave(exception);
            }
        }

        /// <inheritdoc cref="IUnitOfWork.SaveAsync(bool)"/>
        public virtual async Task SaveAsync(bool useChangeTracker = true)
        {
            try
            {
                // TODO: implement a method to automatically save CreatedAt and UpdatedAt fields
                var entries = Context.ChangeTracker.Entries()
                    .Where(x =>
                    {
                        var isInstance = x.InstanceOfType(typeof(BusinessEntity<>));
                        var entity = x.Entity as BusinessEntity<object>;
                        return entity != null;
                    });

                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        // set default values, this is necessary if generator is used to create the api
                        ((IBusinessEntity<object>)entry.Entity).CreatedAt = DateTime.UtcNow;
                    }
                
                    ((IBusinessEntity<object>)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
                
                await Context.UseChangeTracker(useChangeTracker).SaveChangesAsync();

                if (!Is.NullOrEmpty(Transaction))
                {
                    await Task.Run(async () => Transaction.Commit());
                }
            }
            catch (Exception exception)
            {
                await CloseTransactionAsync();
                await ThrowSaveAsync(exception);
            }
        }

        public void CloseTransaction()
        {
            Transaction?.Dispose();
        }

        public async Task CloseTransactionAsync()
        {
            if (!Is.NullOrEmpty(Transaction))
            {
                await Task.Run(async () => Transaction.Dispose());
            }
        }

        /// <summary>
        /// Throw an exception with a more detailed information about the error
        /// </summary>
        /// <param name="exception"></param>
        private void ThrowSave(Exception exception)
        {
            Task.Run(async () => await ThrowSaveAsync(exception)).ConfigureAwait(false);
        }

        /// <summary>
        /// Throw an exception with a more detailed information about the error
        /// </summary>
        /// <param name="exception"></param>
        private async Task ThrowSaveAsync(Exception exception)
        {
            var sb = new StringBuilder();

            switch (exception)
            {
                case DbUpdateConcurrencyException dbUpdateConcurrencyException
                    when exception is DbUpdateConcurrencyException:

                    sb.AppendLine(dbUpdateConcurrencyException.FullMessage());

                    if (!Is.NullOrEmpty(dbUpdateConcurrencyException) &&
                        !Is.NullOrEmpty(dbUpdateConcurrencyException.Entries))
                    {
                        foreach (var eve in dbUpdateConcurrencyException.Entries)
                        {
                            sb.Append("Entity of type ")
                                .Append(eve.Entity.GetType().Name)
                                .Append(" in state ")
                                .Append(eve.State)
                                .AppendLine(" could not be updated");
                        }
                    }

                    Insist.Throw<DbUpdateConcurrencyException>(sb.ToString());
                    break;

                case DbUpdateException dbUpdateException when exception is DbUpdateException:

                    sb.AppendLine(dbUpdateException.FullMessage());

                    if (!Is.NullOrEmpty(dbUpdateException) && !Is.NullOrEmpty(dbUpdateException.Entries))
                    {
                        foreach (var eve in dbUpdateException.Entries)
                        {
                            sb.Append("Entity of type ")
                                .Append(eve.Entity.GetType().Name)
                                .Append(" in state ")
                                .Append(eve.State)
                                .AppendLine(" could not be updated");
                        }

                    }

                    Insist.Throw<DbUpdateException>(sb.ToString());
                    break;

                default:

                    sb.AppendLine(exception.FullMessage());

                    if (!Is.NullOrEmpty(exception) && !Is.NullOrEmpty(exception.Data))
                    {
                        foreach (var eve in exception.Data)
                        {
                            sb.Append("Entity of type ")
                                .AppendLine(eve?.GetType().Name);
                        }
                    }

                    Insist.Throw<Exception>(sb.ToString());
                    break;
            }

            await Task.CompletedTask;
        }
    }
}
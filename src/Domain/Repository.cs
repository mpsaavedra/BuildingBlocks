using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Orun.Domain;
using Orun.Extensions;

namespace Orun.BuildingBlocks.Domain
{
    public class Repository<TKey, TEntity> : IRepository<TKey, TEntity>
        where TEntity : class, IBusinessEntity<TKey>
    {
        public Repository(IMapper mapper, DbContext context)
        {
            Mapper = mapper;
            context.IsNullOrEmpty($"{nameof(context)} could not be null or empty");
            Context = context;
            UnitOfWork = new UnitOfWork<DbContext>(context);
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Query"/>
        public IQueryable<TEntity> Query => Context.ToQueryable<TEntity>();

        /// <inheritdoc cref="IRepository{TKey, TEntity}.UnitOfWork"/>
        public IUnitOfWork UnitOfWork { get; private set; }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Context"/>
        public DbContext Context { get; private set; }
        
        public IMapper Mapper { get; private set; }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Create(TEntity)"/>
        public async Task<TEntity> Create(TEntity input)
        {
            try
            {
                await UnitOfWork.OpenTransactionAsync();
                var entity = Context.Set<TEntity>().Add(input);
                return entity.Entity;
            }
            catch (Exception e)
            {
                await UnitOfWork.CloseTransactionAsync();
                Insist.Throw<ApplicationException>(e.FullMessage());
                throw;
            }
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Get(TKey)"/>
        public async Task<TEntity> Get(TKey id)
        {
            var entity = await Context.Set<TEntity>()
                // .Where(e => e.IsKeyEqualTo(id))
                .FindAsync(id);
                // .FirstOrDefaultAsync(e => e.IsKeyEqualTo(id));

            return await Task.FromResult(entity);
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Get()"/>
        public async Task<IQueryable<TEntity>> Get() =>
            await Task.FromResult(Query.AsQueryable());

        public async Task<IQueryable<TEntity>> Get(string property, Action<PaginationOptions> options)
        {
            var opts = options.ConfigureOrDefault();
            return await Task.FromResult(Query.ToPaged(property, opts.Ascending, opts.Index, opts.Size));
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Get(Expression{Func{TEntity, bool}}, string[])"/>
        public async Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where, params string[] includes) =>
            await Task.FromResult(Query.ToWhere(where).ToInclude(includes));

        /// <inheritdoc cref="IRepository{TKey,TEntity}.Get(Expression{Func{TEntity,bool}},string, Action{PaginationOptions},string[])"/>
        public async Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where, string property,
            Action<PaginationOptions> options, params string[] includes)
        {
            var opts = options.ConfigureOrDefault();
            return await Task.FromResult(Query
                .ToWhere(where)
                .ToInclude(includes)
                .ToPaged(property, opts.Ascending, opts.Index, opts.Size));
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Get(Expression{Func{TEntity, bool}}"/>
        public async Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where) =>
            await Task.FromResult(Query.ToWhere(where));

        public async Task<TEntity> GetOne(Expression<Func<TEntity, bool>> where) =>
            await Query.ToWhere(where).FirstOrDefaultAsync();

        /// <inheritdoc cref="IRepository{TKey,TEntity}.Get(Expression{System.Func{TEntity,bool}},string,System.Action{PaginationOptions})"/>
        public async Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where, string property,
            Action<PaginationOptions> options)
        {
            var opts = options.ConfigureOrDefault();
            return await Task.FromResult(Query
                .ToWhere(where)
                .ToPaged(property, opts.Ascending, opts.Index, opts.Size));
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Get(string[])"/>
        public async Task<IQueryable<TEntity>> Get(params string[] includes) =>
            await Task.FromResult(Query.ToInclude(includes));

        public async Task<IQueryable<TEntity>> Get(string property, Action<PaginationOptions> options,
            params string[] includes)
        {
            var opts = options.ConfigureOrDefault();
            return await Task.FromResult(Query
                .ToInclude(includes)
                .ToPaged(property, opts.Ascending, opts.Index, opts.Size));
        }

        /// <summary>
        /// Updates a given entity
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task Update(TKey id, TEntity input)
        {
            try
            {
                await UnitOfWork.OpenTransactionAsync();
                var entity = await Get(id);
                entity.IsNullOrEmpty($"entity with id {id} could was not found");
                Context.Set<TEntity>().Update(entity);
            }
            catch (Exception e)
            {
                await UnitOfWork.CloseTransactionAsync();
                Insist.Throw<ApplicationException>(e.FullMessage());
            }
        }

        /// <inheritdoc cref="IRepository{TKey, TEntity}.Delete(TKey)"/>
        public async Task Delete(TKey id)
        {
            try
            {
                await UnitOfWork.OpenTransactionAsync();
                var entity = await Context.Set<TEntity>().FindAsync(id);
                Context.Set<TEntity>().Remove(entity);
            }
            catch (Exception e)
            {
                await UnitOfWork.CloseTransactionAsync();
                Insist.Throw<ApplicationException>(e.FullMessage());
            }
        }

        /// <summary>
        /// returns if theres any result
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Any(Expression<Func<TEntity, bool>> where) =>
            await Query.AnyAsync(where);

        /// <summary>
        /// returns if theres any result
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Any() =>
            await Query.AnyAsync();
    }
}
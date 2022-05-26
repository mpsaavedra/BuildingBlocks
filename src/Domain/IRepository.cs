using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Orun.Domain;

namespace Orun.BuildingBlocks.Domain
{
    /// <summary>
    /// repository definition, defines the basic operations related with access to data and other
    /// data related functionalities.
    /// </summary>
    /// <typeparam name="TEntity">Entity to use from the Application database context</typeparam>
    /// <typeparam name="TKey">Id type to use within the entity</typeparam>
    public interface IRepository<TKey, TEntity>
        where TEntity : class, IBusinessEntity<TKey>
    {
        /// <summary>
        /// Query result
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        /// Unit of work
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// Application database context
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// AutoMapper mapping interface
        /// </summary>
        IMapper Mapper { get; }

        /// <summary>
        /// Create a new entity and returns the Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TEntity> Create(TEntity input);

        /// <summary>
        /// returns the one entity that has provided Id
        /// </summary>
        /// <returns></returns>
        Task<TEntity> Get(TKey id);

        /// <summary>
        /// returns all available entities
        /// </summary>
        /// <returns></returns>
        Task<IQueryable<TEntity>> Get();
        
        /// <summary>
        /// returns a paginated list of all entities
        /// </summary>
        /// <param name="property">property to order by, default is Id</param>
        /// <param name="pagedOptions"><see cref="PaginationOptions"/> <see cref="Action{T}"/> with pagination parameters</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> Get(string property, Action<PaginationOptions> pagedOptions);

        /// <summary>
        /// Returns a list of elements and include foreign related entities. Included values are located
        /// using string with dot notation.
        /// </summary>
        /// <example>
        /// var data = _ClientRepository.get( x => x.Id == 10, "Country", "Country.State");
        /// </example>
        /// <param name="where">Expression to filter entities</param>
        /// <param name="includes">string array with the foreign related entities to include</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where, params string[] includes);
        Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where, string property, 
            Action<PaginationOptions> options, params string[] includes);

        /// <summary>
        /// returns a list of elements
        /// </summary>
        /// <param name="where">function Expression to filter entities</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where);
        Task<TEntity> GetOne(Expression<Func<TEntity, bool>> where);
        Task<IQueryable<TEntity>> Get(Expression<Func<TEntity, bool>> where, string property, Action<PaginationOptions> options);

        /// <summary>
        /// returns a list of elements
        /// </summary>
        /// <param name="includes">string array with the foreign related entities to include</param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> Get(params string[] includes);
        Task<IQueryable<TEntity>> Get(string property, Action<PaginationOptions> options, params string[] includes);

        /// <summary>
        /// updates an entity value
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Update(TKey id, TEntity input);

        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(TKey id);

        /// <summary>
        /// Returns if theres any result
        /// </summary>
        /// <returns></returns>
        Task<bool> Any(Expression<Func<TEntity, bool>> where);

        /// <summary>
        /// Returns if theres any result
        /// </summary>
        /// <returns></returns>
        Task<bool> Any();

    }
}
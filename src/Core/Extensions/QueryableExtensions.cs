using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Orun.Extensions
{
    /// <summary>
    /// Queryable extensions
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> object that with disable LazyLoading and CHange tracking
        /// to make query faster.
        /// </summary>
        /// <typeparam name="T">Entity type to query.</typeparam>
        /// <param name="context">Db context</param>
        /// <returns>IQueryable{T} | ApplicationException: if context is null.</returns>
        public static IQueryable<T> ToQueryable<T>(this DbContext context)
            where T : class
            => context.Set<T>();
            // => context.UseChangeTracker(false).Set<T>();

        /// <summary>
        /// Includes related entities to the query 
        /// </summary>
        /// <typeparam name="T">Entity type to query</typeparam>
        /// <param name="queryable">Query of context</param>
        /// <param name="includes">Entities to include.</param>
        /// <returns>IQueryable{T}.</returns>
        public static IQueryable<T> ToInclude<T>(this IQueryable<T> queryable, params Expression<Func<T, object>>[] includes)
            where T : class
        {
            includes?.ToList().ForEach(include => queryable = queryable.Include(include));

            return queryable;
        }
        
        /// <summary>
        /// includes related entities to the query result. It allows to include sub properties. The LinQ function
        /// <b>Include</b> allows to include navigation properties represented as strings.
        /// </summary>
        /// <example>
        ///     var users = ApplicationDbContext.Users.ToInclude("Name", "Address", "WareHouse.Name");
        /// </example>
        /// <param name="queryable"></param>
        /// <param name="includes">string array of foreign related entities</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> ToInclude<T>(this IQueryable<T> queryable, params string[] includes)
            where T: class
        {
            includes?.ToList().ForEach(include => queryable = queryable.Include(include));
            return queryable;
        }

        /// <summary>
        /// list a list based on a predicate
        /// </summary>
        /// <typeparam name="T">list type.</typeparam>
        /// <param name="queryable"><see cref="IQueryable{T}"/> to filter.</param>
        /// <param name="where">filter predicate to apply.</param>
        /// <returns>List with applied filter.</returns>
        public static IQueryable<T> ToWhere<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> where = null!)
            where T : class
        {
            if (where != default)
            {
                queryable = queryable.Where(where);
            }

            return queryable;
        }

        /// <summary>
        /// Paginates a list of results
        /// </summary>
        /// <typeparam name="T">list Type.</typeparam>
        /// <param name="queryable"><see cref="IQueryable{T}"/> object with result.</param>
        /// <param name="property">field name to order by</param>
        /// <param name="ascending">if true ascending</param>
        /// <param name="index">in which page the list begins</param>
        /// <param name="size">amount of rows to include</param>
        /// <returns>Paged list</returns>
        public static IQueryable<T> ToPaged<T>(
            this IQueryable<T> queryable,
            string property,
            bool ascending = true,
            int index = 1,
            int size = 20)
        {
            if (Is.NullOrAnyNull(queryable))
            {
                return queryable;
            }

            if (!string.IsNullOrWhiteSpace(property))
            {
                queryable = queryable.ToOrder(property, ascending);
            }

            queryable.Skip((index - 1) * size).Take(size);

            return queryable;
        }

        /// <summary>
        /// returns an ordered list with the results
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="property"></param>
        /// <param name="ascending"></param>
        /// <typeparam name="T">result type of result</typeparam>
        /// <returns></returns>
        private static IQueryable<T> ToOrder<T>(
            this IQueryable<T> queryable,
            string property,
            bool ascending)
        {
            queryable.IsNullOrAnyNull(nameof(queryable));
            property.IsNullOrEmpty(nameof(property));

            var properties = property.Split('.');

            Type propertyType = typeof(T);

            propertyType = properties.Aggregate(propertyType, (Type propertyTypeCurrent, string propertyName) =>
                propertyTypeCurrent?.GetProperty(propertyName)?.PropertyType!);

            if (propertyType == typeof(sbyte))
            {
                return queryable.ToOrder<T, sbyte>(properties, ascending);
            }

            if (propertyType == typeof(short))
            {
                return queryable.ToOrder<T, short>(properties, ascending);
            }

            if (propertyType == typeof(int))
            {
                return queryable.ToOrder<T, int>(properties, ascending);
            }

            if (propertyType == typeof(long))
            {
                return queryable.ToOrder<T, long>(properties, ascending);
            }

            if (propertyType == typeof(byte))
            {
                return queryable.ToOrder<T, byte>(properties, ascending);
            }

            if (propertyType == typeof(ushort))
            {
                return queryable.ToOrder<T, ushort>(properties, ascending);
            }

            if (propertyType == typeof(uint))
            {
                return queryable.ToOrder<T, uint>(properties, ascending);
            }

            if (propertyType == typeof(ulong))
            {
                return queryable.ToOrder<T, ulong>(properties, ascending);
            }

            if (propertyType == typeof(char))
            {
                return queryable.ToOrder<T, char>(properties, ascending);
            }

            if (propertyType == typeof(float))
            {
                return queryable.ToOrder<T, float>(properties, ascending);
            }

            if (propertyType == typeof(double))
            {
                return queryable.ToOrder<T, double>(properties, ascending);
            }

            if (propertyType == typeof(decimal))
            {
                return queryable.ToOrder<T, decimal>(properties, ascending);
            }

            if (propertyType == typeof(bool))
            {
                return queryable.ToOrder<T, bool>(properties, ascending);
            }

            if (propertyType == typeof(string))
            {
                return queryable.ToOrder<T, string>(properties, ascending);
            }

            return queryable.ToOrder<T, object>(properties, ascending);
        }

        /// <summary>
        /// returns an ordered list by a list of properties
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="properties"></param>
        /// <param name="ascending"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        private static IQueryable<T> ToOrder<T, TKey>(
            this IQueryable<T> queryable,
            IEnumerable<string> properties,
            bool ascending)
        {
            queryable.IsNullOrAnyNull(nameof(queryable));
            properties.IsNullOrAnyNull(nameof(properties));

            var parameters = Expression.Parameter(typeof(T));

            var body = properties.Aggregate<string, Expression>(parameters, Expression.Property);

            var expression = Expression.Lambda<Func<T, TKey>>(body, parameters).Compile();

            return ascending
                ? queryable.AsEnumerable().OrderBy(expression).AsQueryable()
                : queryable.AsEnumerable().OrderByDescending(expression).AsQueryable();
        }
        
        /// <summary>
        /// returns the ordered and paged list of entities
        /// </summary>
        /// <param name="entities">this list of elements to order</param>
        /// <param name="sortBy"></param>
        /// <param name="order"></param>
        /// <param name="ascending"></param>
        /// <param name="offset"></param>
        /// <param name="take"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> GetEntityOrdered<T>(this IQueryable<T> entities, string sortBy, string order,
            bool ascending, int offset, int take)
        {
            return entities.ToPaged(sortBy, ascending, offset, take);
        }
        
        /// <summary>
        /// return the paged and ordered list of entities
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="sortBy"></param>
        /// <param name="order"></param>
        /// <param name="offset"></param>
        /// <param name="take"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> GetEntityOrdered<T>(this IQueryable<T> entities, string sortBy,
            string order, int offset, int take)
        {

            return entities.ToPaged(sortBy, true, offset, take);
        }
    }
}
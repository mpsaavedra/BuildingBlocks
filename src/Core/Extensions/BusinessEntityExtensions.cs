using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Orun.Domain;

namespace Orun.Extensions
{
    /// <summary>
    /// Trackable object extension
    /// </summary>
    public static class BusinessEntityExtensions
    {
        /// <summary>
        /// returns true if the value of the Id value for the <see cref="BusinessEntity{TKey}"/> object
        /// is equal to the provided value. It will consider as the Id of the entity the value that is
        /// annotated with <see cref="KeyAttribute"/> or it will check for all properties that implements
        /// the TKey type which name could be Id, ID or [entity_name]ID. Then it compares the property
        /// value with the provided value.
        /// </summary>
        /// <param name="entity">this trackable object</param>
        /// <param name="valueToCompare"></param>
        /// <typeparam name="TKey">type of the Id member</typeparam>
        /// <returns>value of the Id field</returns>
        public static bool IsKeyEqualTo<TKey>(this BusinessEntity<TKey> entity, TKey valueToCompare)
        {
            return entity.PropertiesOfType(typeof(TKey))
                .Where(prop =>
                {
                    // property has Key attribute or is Id or name is <entityName>Id
                    if (prop.GetCustomAttributes(true).Any(attr => attr.InstanceOfType(typeof(KeyAttribute))) ||
                        prop.Name.ToLower() == "id" ||
                        prop.Name.ToLower().EndsWith(entity.GetType().Name.ToLower() + "id"))
                    {
                        return Equals(prop.GetValue(entity), valueToCompare);
                    }

                    return false;
                })
                .Any();
        }

        /// <summary>
        /// returns the value of the Id value for the <see cref="BusinessEntity{TKey}"/> object.
        /// It will consider as the Id of the entity the value that is annotated with
        /// <see cref="KeyAttribute"/> or it will check for all properties that implements
        /// the TKey type which name could be Id, ID or [entity_name]ID.
        /// </summary>
        /// <param name="entity">this trackable object</param>
        /// <typeparam name="TKey">type of the Id member</typeparam>
        /// <returns>value of the Id field</returns>
        public static TKey GetKeyValue<TKey>(this BusinessEntity<TKey> entity)
        {
            var prop = entity
                .PropertiesOfType(typeof(TKey))
                .FirstOrDefault(prop => prop
                    .GetCustomAttributes(true)
                    .Any(attr => attr.InstanceOfType(typeof(KeyAttribute))) ||
                                        prop.Name.ToLower() == "id" ||
                                        prop.Name.ToLower().EndsWith(entity.GetType().Name.ToLower() + "id"));
            if (prop != null)
                return (TKey)prop.GetValue(entity)!;
            return default(TKey)!;
        }
    }
}
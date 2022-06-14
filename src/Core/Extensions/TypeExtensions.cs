using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orun.Extensions
{
    /// <summary>
    /// <see cref="Type"/> extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// returns true
        /// </summary>
        /// <param name="type"></param>
        /// <param name="checkType"></param>
        /// <returns></returns>
        public static bool OfType(this Type type, Type checkType)
        {
            return type.ToString() == checkType.ToString();
        }
        
        /// <summary>
        /// Returns an IEnumerable of <see cref="PropertyInfo"/> with the Property
        /// that correspond with a given Type of Attribute
        /// </summary>
        /// <returns>The properties with attribute.</returns>
        /// <param name="t">Type base.</param>
        /// <typeparam name="TAttr">The 1st type parameter.</typeparam>
        /// <example>
        /// <code>
        ///   MyType
        ///     .GetPropertiesWithAttribute{ApiNullifyOnCreateAttribute}()
        ///     .Select(t => { /*  do something */ });
        /// </code>
        /// </example>
        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute<TAttr>(this Type t)
            where TAttr : Attribute
        { 
            List<PropertyInfo> result = new List<PropertyInfo>();

            foreach (var propertyInfo in t.GetProperties())
            {
                foreach (var attribute in propertyInfo.GetCustomAttributes(true))
                {
                    if(attribute.InstanceOfType(typeof(TAttr)) && !result.Contains(propertyInfo))
                    {
                        result.Add(propertyInfo);
                    }
                }
            }
                
            return result;
        }
    }
}
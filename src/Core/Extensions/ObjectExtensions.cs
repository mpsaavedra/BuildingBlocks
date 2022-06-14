using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace Orun.Extensions
{
    /// <summary>
    /// clr object type extensions
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// returns true if the objects is of provided type
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static bool InstanceOfType(this object obj, Type objectType)
        {
            return obj.GetType().OfType(objectType);
        }


        /// <summary>
        /// returns all properties of a given type
        /// </summary>
        /// <param name="obj">this object</param>
        /// <param name="type">type of properties</param>
        /// <returns>a collection of PropertyInfo with properties</returns>
        public static IEnumerable<PropertyInfo> PropertiesOfType(this object obj, Type type) =>
            obj.GetType().GetProperties().Where(p => p.PropertyType == type);

        /// <summary>
        /// Populate an object with data from another object keeping the data of the original
        /// object if properties does not appear or are null or empty.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mappedData"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource PopulateWithMappedData<TSource>(this TSource source, object mappedData)
        {
            foreach (var dbProperty in source!.GetType().GetProperties())
            {
                if(mappedData.GetType().GetProperties().Any(p => p.Name == dbProperty.Name))
                {
                    var mappedValue = mappedData.GetType().GetProperty(dbProperty.Name)?.GetValue(mappedData);
                    var sourceValue = source.GetType().GetProperty(dbProperty.Name)?.GetValue(source);
                    sourceValue = mappedValue.ThenIfNullOrEmpty(sourceValue);
                    source
                        .GetType()
                        .GetProperty(dbProperty.Name)?
                        .SetValue(source, sourceValue);
                }
            }

            return source;
        }
    }
}
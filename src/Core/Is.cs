using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orun.Extensions;

namespace Orun
{
    /// <summary>
    /// Validations among other utilities functions and extensions
    /// </summary>
    public static class Is
    {
        /// <summary>
        /// returns true if any of the provided values is Null or Empty
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool NullOrEmpty(params object[] values)
        {
            var fails = values.Where(value =>
            {
                if (value == null)
                {
                    return true;
                }

                if (value.InstanceOfType(typeof(string)))
                {
                    return string.IsNullOrEmpty(((string)value));
                }

                return false;
            });

            return fails.Any();
        }

        /// <summary>
        /// returns true if the source is nullable
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static bool IsNullable<TSource>(this TSource source)
        {
            if (source == null) return true;
            Type type = typeof(TSource);
            if (!type.IsValueType) return true;
            if (Nullable.GetUnderlyingType(type) != null) return true;
            return false;
            // return default(TSource) == null;
        }

        /// <summary>
        /// checks if source is null or empty if it is, throws and exception
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource IsNullOrEmpty<TSource>(this TSource source, string message) =>
            source.Throw(() => NullOrEmpty(source!), message);

        /// <summary>
        /// if source is null use set the optional value
        /// </summary>
        /// <param name="source"></param>
        /// <param name="optional"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource ThenIfNullOrEmpty<TSource>(this TSource source, TSource optional)
        {
            if (NullOrEmpty(source!))
                return optional;
            if (!source.IsNullable())
                return source;
            return source;
            
            // !NullOrEmpty(source) ? (
            //     source.IsNullable() ? source : source
            // ) : optional;
        }
        

        /// <summary>
        /// Check if values are not null and contain any element.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool NullOrAnyNull(params object[] values)
        {
            var fails = values.Where(value =>
            {
                if (value == null)
                {
                    return true;
                }

                if (value.InstanceOfType(typeof(string)))
                {
                    return ((string)value).Length < 1;
                }

                if (value.InstanceOfType(typeof(Array)))
                {
                    return ((Array)value).Length < 1;
                }

                if (value.InstanceOfType(typeof(ICollection<>)))
                {
                    return ((ICollection)value).Count < 1;
                }

                // because is not one of evaluated Types
                return true;
            });

            return fails.Any();
        }

        /// <summary>
        /// check if any of provided values is null or has a null value, if true it will
        /// launch an exception
        /// </summary>
        /// <param name="source"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource IsNullOrAnyNull<TSource>(this TSource source) =>
            source.Throw(() => Is.NullOrAnyNull(source!), 
                $"{nameof(source)} could not be null or contain a null value");

        /// <summary>
        /// check if any of provided values is null or has a null value, if true it will
        /// launch an exception with the provided message.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource IsNullOrAnyNull<TSource>(this TSource source, string message) =>
            source.Throw(() => Is.NullOrAnyNull(source!), message);

        /// <summary>
        /// Check if the source and predicate are not null, that the result of the
        /// predicate evaluation function is false. if any of this is true it will Throw an
        /// <see cref="ApplicationException"/>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="func"></param>
        /// <param name="message"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static TSource Throw<TSource>(this TSource source, Func<bool> func, string message)
        {
            if (source.Errors(func))
            {
                Insist.Throw<ApplicationException>(message);
            }

            return source;
        }

        /// <summary>
        /// Check if the source and predicate are not null and the result of the
        /// predicate evaluation function is false
        /// </summary>
        /// <param name="source">value to evaluate</param>
        /// <param name="func">function to evaluate</param>
        /// <typeparam name="TSource">Type of value to evaluate</typeparam>
        /// <returns>true if there are any null value false if not</returns>
        public static bool Errors<TSource>(this TSource source, Func<bool> func) =>
            source == null || func == null || func();
    }
}
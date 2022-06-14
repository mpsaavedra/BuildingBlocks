using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Serilog;

namespace Orun
{
    /// <summary>
    /// Conditional checks with Exception launcher. It throws the exception but it keeps
    /// a list of cached exceptions that allows to debug the exception list.
    /// <remarks>
    /// It could crash if you are testing your app using NUnit
    /// </remarks>
    /// </summary>
    public static class Insist
    {
        /// <summary>
        /// throws and exception if the provided item is null
        /// </summary>
        /// <param name="item">item to check that's not null</param>
        /// <param name="parameterName"></param>
        /// <typeparam name="TItem">type of item to check for null</typeparam>
        public static void IsNotNull<TItem>(TItem item, string parameterName)
        {
            if (item is null)
            {
                Throw<ArgumentNullException>(parameterName);
            }
        }

        /// <summary>
        /// throws an exception if the provided item is null or whitespace
        /// </summary>
        /// <param name="item">item to check if null or whitespace</param>
        /// <param name="parameterName"></param>
        public static void IsNotNullOrWhiteSpace(string item, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
                Throw<ArgumentNullException>($"Parameter {parameterName} is null or whitespace");
            }
        }

        /// <summary>
        /// Launch the provided <typeparamref name="TException"/> if the condition is
        /// not correct, and launch the message function provided to notify about it
        /// </summary>
        /// <param name="condition">If set to <c>true</c> condition.</param>
        /// <param name="msg">Message.</param>
        /// <typeparam name="TException">The 1st type parameter.</typeparam>
        public static void True<TException>(bool condition, Func<string> msg) where TException : Exception
        {
            if (!condition)
                Throw<TException>(msg());
        }

        /// <summary>
        /// Launch the provided <typeparamref name="TException"/> if the condition is not
        /// not correct, and launch the message function provided to notify about it
        /// </summary>
        /// <param name="condition">If set to <c>false</c> condition.</param>
        /// <param name="msg">Message.</param>
        /// <typeparam name="TException">The 1st type parameter.</typeparam>
        public static void False<TException>(bool condition, Func<string> msg) where TException : Exception
        {
            True<TException>(!condition, msg);
        }

        /// <summary>
        /// Gets the cached exception types that has been launched.
        /// </summary>
        /// <value>The cached exception types.</value>
        public static IEnumerable<Type> CachedExceptionTypes
        {
            get
            {
                lock (LockObject)
                {
                    return EncounteredExceptionTypes.Keys.ToArray();
                }
            }
        }

        /// <summary>
        /// Throw the specified <typeparamref name="TException"/> with the
        /// provided message.
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <typeparam name="TException">The 1st type parameter.</typeparam>
        public static void Throw<TException>(string msg) where TException : Exception
        {
            ConstructorInfo info;
            // lock the object to make it thread safe
            lock (LockObject)
            {
                // build the constructor
                var t = typeof(TException);
                if (EncounteredExceptionTypes.ContainsKey(t))
                {
                    info = EncounteredExceptionTypes[t];
                }
                else
                {
                    info = t.GetConstructor(new[] { typeof(string) })!;

                    // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                    if (info != null)
                    {
                        EncounteredExceptionTypes[t] = info;
                    }
                }
            }
            
            Log.Error(msg);
            
            // invoke constructor and throw the exception
// #pragma warning disable CS8597 // Throw value may be null
            throw (info?.Invoke(new[] { msg }) as TException);
// #pragma warning restore CS8597 // Throw value may be null
        }

        /// <summary>
        /// Throw the specified <typeparamref name="TException"/> with the
        /// provided message.
        /// </summary>
        /// <param name="msgs"></param>
        /// <typeparam name="TException"></typeparam>
        public static void Throw<TException>(IEnumerable<string> msgs) where TException : Exception
        {
            if (!(msgs?.Count() > 0))
            {
                return;
            }

            var error = msgs.Aggregate("", (current, msg) => current + (current + ","));

            
            Throw<TException>(error);
        }

        /// <summary>
        /// register an exception into the list 
        /// </summary>
        /// <param name="exception"></param>
        /// <typeparam name="TException"></typeparam>
        public static void RegisterException<TException>(TException exception)
        {
            ConstructorInfo info;
            // lock the object to make it thread safe
            lock (LockObject)
            {
                // build the constructor
                var t = typeof(TException);
                if (EncounteredExceptionTypes.ContainsKey(t))
                {
                    info = EncounteredExceptionTypes[t];
                }
                else
                {
                    info = t.GetConstructor(new[] { typeof(string) })!;

                    if (info != null)
                    {
                        EncounteredExceptionTypes[t] = info;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the encountered exception types.
        /// </summary>
        /// <value>The encountered exception types.</value>
        private static Dictionary<Type, ConstructorInfo> EncounteredExceptionTypes { get; set; } =
            new Dictionary<Type, ConstructorInfo>();

        private static object LockObject = new object();
    }
}
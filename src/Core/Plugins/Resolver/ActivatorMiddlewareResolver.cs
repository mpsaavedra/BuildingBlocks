using System;

namespace Orun.Plugins.Resolver
{
    /// <summary>
    /// A very simple middleware resolver that uses <see cref="Activator"/>
    /// </summary>
    public class ActivatorMiddlewareResolver : IMiddlewareResolver
    {
        /// <summary>
        /// creates a new instance of the provided type using <see cref="Activator"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type)!;
        }
    }
}
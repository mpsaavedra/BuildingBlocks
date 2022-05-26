using System;

namespace Orun.Plugins.Resolver
{
    public class ActivatorMiddlewareResolver : IMiddlewareResolver
    {
        public object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
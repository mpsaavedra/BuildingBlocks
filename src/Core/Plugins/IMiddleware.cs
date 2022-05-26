using System;

namespace Orun.Plugins
{
    /// <summary>
    /// define a new middleware information, that returns a value
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public interface IMiddleware<TParameter, TReturn>
    {
        /// <summary>
        /// execute the middleware and return value
        /// </summary>
        /// <param name="parameter">input data</param>
        /// <param name="next">next middleware</param>
        /// <returns></returns>
        TReturn Run(TParameter parameter, Func<TParameter, TReturn> next);
    }

    /// <summary>
    /// define a new middleware information, that does not return a value
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IMiddleware<TParameter>
    {
        /// <summary>
        /// execute the middleware but it does not return any value
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="next">next middleware</param>
        void Run(TParameter parameter, Action<TParameter> next);
    }
}
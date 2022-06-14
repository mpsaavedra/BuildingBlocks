using System;
using System.Threading.Tasks;

namespace Orun.Plugins
{
    /// <summary>
    /// define a new middleware information, this middleware return information
    /// </summary>
    /// <typeparam name="TParameter">input data</typeparam>
    /// <typeparam name="TReturn">output data</typeparam>
    public interface IAsyncMiddleware<TParameter, TReturn>
    {
        /// <summary>
        /// Runs the middleware
        /// </summary>
        /// <param name="parameter">parameter to pass to the middleware</param>
        /// <param name="next">next middleware to execute</param>
        /// <returns></returns>
        Task<TReturn> Run(TParameter parameter, Func<TParameter, Task<TReturn>> next);
    }

    /// <summary>
    /// defines the basic structure of an async middleware
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    public interface IAsyncMiddleware<TParameter>
    {
        /// <summary>
        /// runs the middleware
        /// </summary>
        /// <param name="parameter">parameter passed to the middleware</param>
        /// <param name="next">next middleware</param>
        /// <returns></returns>
        Task Run(TParameter parameter, Func<TParameter, Task> next);
    }
}
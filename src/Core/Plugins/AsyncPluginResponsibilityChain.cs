using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orun.Plugins
{
    /// <summary>
    /// implements the <see cref="IAsyncPluginResponsibilityChain{TParameter,TReturn}"/> interface
    /// with the chain processes and all that
    /// </summary>
    /// <typeparam name="TParameter"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class AsyncPluginResponsibilityChain<TParameter, TReturn> : IAsyncPluginResponsibilityChain<TParameter, TReturn>
    {
        private List<object> _middlewares;
        private Func<TParameter, Task<TReturn>>? _finallyFunc = null;

        /// <summary>
        /// returns a new instance of <see cref="AsyncResponsibilityChain{TParameter,TReturn}"/>
        /// </summary>
        public AsyncPluginResponsibilityChain()
        {
            _middlewares = new List<object>();
        }

        public AsyncPluginResponsibilityChain<TParameter, TReturn> Chain(object instance)
        {
            _middlewares.Add(instance);
            return this;
        }

        /// <summary>
        /// execute the async middleware hain
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<TReturn> Execute(TParameter parameter)
        {
            if (_middlewares.Count == 0)
            {
                return default(TReturn)!;
            }

            int index = 0;
            Func<TParameter, Task<TReturn>> func = null!;
            func = (param) =>
            {
                var instance = _middlewares[index];
                var middleware = (IAsyncMiddleware<TParameter, TReturn>) instance;

                index++;
                if (index == _middlewares.Count)
                {
                    func = _finallyFunc ?? ((p) => 
                        Task.FromResult(default(TReturn))!);
                }

                return middleware.Run(param, func);
            };

            return await func(parameter).ConfigureAwait(false);
        }

        public AsyncPluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, Task<TReturn>> finallyFunc)
        {
            _finallyFunc = finallyFunc;
            return this;
        }
    }
}
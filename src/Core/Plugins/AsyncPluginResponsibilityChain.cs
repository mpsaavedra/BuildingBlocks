using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orun.Plugins
{
    public class AsyncPluginResponsibilityChain<TParameter, TReturn> : IAsyncPluginResponsibilityChain<TParameter, TReturn>
    {
        private List<object> _middlewares;
        private Func<TParameter, Task<TReturn>> _finallyFunc;

        public AsyncPluginResponsibilityChain()
        {
            _middlewares = new List<object>();
        }

        public AsyncPluginResponsibilityChain<TParameter, TReturn> Chain(object instance)
        {
            _middlewares.Add(instance);
            return this;
        }

        public async Task<TReturn> Execute(TParameter parameter)
        {
            if (_middlewares.Count == 0)
            {
                return default(TReturn);
            }

            int index = 0;
            Func<TParameter, Task<TReturn>> func = null;
            func = (param) =>
            {
                var instance = _middlewares[index];
                var middleware = (IAsyncMiddleware<TParameter, TReturn>) instance;

                index++;
                if (index == _middlewares.Count)
                {
                    func = _finallyFunc ?? ((p) => 
                        Task.FromResult(default(TReturn)));
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
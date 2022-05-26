using System;
using System.Collections.Generic;

namespace Orun.Plugins
{
    public class PluginResponsibilityChain<TParameter, TReturn>: IPluginResponsibilityChain<TParameter, TReturn>
    {
        private List<object> _middlewares;
        private Func<TParameter, TReturn> _finallyFunc;

        public PluginResponsibilityChain()
        {
            _middlewares = new List<object>();
        }

        public PluginResponsibilityChain<TParameter, TReturn> Chain(object instance)
        {
            _middlewares.Add(instance);
            return this;
        }
        
        public TReturn Execute(TParameter parameter)
        {
            if (_middlewares.Count == 0)
            {
                return default(TReturn);
            }

            int index = 0;
            Func<TParameter, TReturn> func = null;
            func = (param) =>
            {
                var instance = _middlewares[index];
                var middleware = (IMiddleware<TParameter, TReturn>) instance;

                index++;
                if (index == _middlewares.Count)
                {
                    func = _finallyFunc ?? ((p) => 
                        default(TReturn));
                }

                return middleware.Run(param, func);
            };

            return func(parameter);
        }

        public PluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFun)
        {
            _finallyFunc = finallyFun;
            return this;
        }
    }
}
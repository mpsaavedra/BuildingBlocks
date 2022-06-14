using System;
using System.Collections.Generic;

namespace Orun.Plugins
{
    public class PluginResponsibilityChain<TParameter, TReturn>: IPluginResponsibilityChain<TParameter, TReturn>
    {
        private List<object> _middlewares;
        private Func<TParameter, TReturn>? _finallyFunc = null;

        /// <summary>
        /// returns a new instance of <see cref="PluginResponsibilityChain{TParameter,TReturn}"/>
        /// </summary>
        public PluginResponsibilityChain()
        {
            _middlewares = new List<object>();
        }

        /// <summary>
        /// includes the new object into the middlewarae chain
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public PluginResponsibilityChain<TParameter, TReturn> Chain(object instance)
        {
            _middlewares.Add(instance);
            return this;
        }
        
        /// <summary>
        /// executes the middleware chain
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public TReturn Execute(TParameter parameter)
        {
            if (_middlewares.Count == 0)
            {
                return default(TReturn)!;
            }

            int index = 0;
            Func<TParameter, TReturn> func = null!;
            func = (param) =>
            {
                var instance = _middlewares[index];
                var middleware = (IMiddleware<TParameter, TReturn>) instance;

                index++;
                if (index == _middlewares.Count)
                {
                    func = _finallyFunc ?? ((p) => 
                        default(TReturn)!);
                }

                return middleware.Run(param, func);
            };

            return func(parameter);
        }

        /// <summary>
        /// sets a function to execute after the chain has been finished
        /// </summary>
        /// <param name="finallyFun"></param>
        /// <returns></returns>
        public PluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFun)
        {
            _finallyFunc = finallyFun;
            return this;
        }
    }
}
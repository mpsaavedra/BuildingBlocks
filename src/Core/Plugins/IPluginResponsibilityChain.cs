using System;

namespace Orun.Plugins
{
    public interface IPluginResponsibilityChain<TParameter, TReturn>
    {
        PluginResponsibilityChain<TParameter, TReturn> Chain(object instance);

        TReturn Execute(TParameter parameter);

        PluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, TReturn> finallyFun);
    }
}
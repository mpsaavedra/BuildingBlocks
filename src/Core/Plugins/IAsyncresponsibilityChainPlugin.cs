using System;
using System.Threading.Tasks;

namespace Orun.Plugins
{
    public interface IAsyncPluginResponsibilityChain<TParameter, TReturn>
    {
        AsyncPluginResponsibilityChain<TParameter, TReturn> Chain(object instance);

        Task<TReturn> Execute(TParameter parameter);

        AsyncPluginResponsibilityChain<TParameter, TReturn> Finally(Func<TParameter, Task<TReturn>> finallyFunc);
        
    }
}
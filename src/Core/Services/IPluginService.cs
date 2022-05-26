using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Orun.Plugins;

namespace Orun.Services
{
    /// <summary>
    /// Dependency Injection service to handle <see cref="IPlugin"/> instances.
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// Configure plugins in the Application builder method
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        IApplicationBuilder ConfigurePlugins(IApplicationBuilder app);
        
        /// <summary>
        /// load plugin from a plugin file and shared types
        /// </summary>
        /// <param name="pluginFile"></param>
        /// <param name="sharedTypes"></param>
        void LoadPlugin(string pluginFile, Type[] sharedTypes);

        /// <summary>
        /// load plugins from given directory
        /// </summary>
        /// <param name="pluginsDirectory"></param>
        /// <param name="sharedTypes"></param>
        void LoadPlugins(string pluginsDirectory, Type[] sharedTypes);

        /// <summary>
        /// returns a given plugin located within his <see cref="IPlugin.PluginId"/>. It
        /// will search in the plugins first and after in the middleware list.
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="TPlugin"></typeparam>
        /// <returns></returns>
        Task<TPlugin> Get<TPlugin>(Guid guid);

        /// <summary>
        /// returns a given plugin located within his <see cref="IPlugin.ShortName"/>. It
        /// will search in the plugins first and after in the middleware list.
        /// </summary>
        /// <param name="shortName"></param>
        /// <typeparam name="TPlugin"></typeparam>
        /// <returns></returns>
        Task<TPlugin> Get<TPlugin>(string shortName);

        /// <summary>
        /// returns the chain
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        Task<ICollection<IMiddlewarePlugin>> MiddlewareChain(string eventCode);
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using McMaster.NETCore.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orun.Plugins;

namespace Orun.Services
{
    // must be services.AddSingleton<IPluginService, PluginService>();
    public partial class PluginService : IPluginService
    {
        private List<IPlugin> _plugins;
        private List<IMiddlewarePlugin> _noEventMiddlewares;
        private Dictionary<string, ICollection<IMiddlewarePlugin>> _middlewares;

        
        /// <summary>
        /// returns a new instance of <see cref="PluginService"/>. It load plugins from the directory recursively
        /// and configures plugins services.
        /// </summary>
        /// <param name="pluginsDir">directory where plugins are located</param>
        /// <param name="sharedTypes">types to shared with the plugins this ensures that the plugin resolves to the
        /// same version of DependencyInjection and ASP.NET Core that the current app uses</param>
        /// <param name="services">Service collection to configure plugins</param>
        public PluginService(string pluginsDir, Type[] sharedTypes, IServiceCollection services)
        {
            _plugins = new List<IPlugin>();
            _noEventMiddlewares = new List<IMiddlewarePlugin>();
            _middlewares = new Dictionary<string, ICollection<IMiddlewarePlugin>>();
            LoadPlugins(pluginsDir, sharedTypes);
            ConfigurePlugins(services);
        }

        /// <summary>
        /// functional plugins list
        /// </summary>
        public List<IPlugin> Plugins => _plugins;

        /// <summary>
        /// Middleware plugins
        /// </summary>
        public Dictionary<string, ICollection<IMiddlewarePlugin>> MiddlewarePlugins => _middlewares;

        /// <summary>
        /// Load a plugins from a file and shared  types. When loading it will
        /// load differently <see cref="IMiddlewarePlugin"/> from <see cref="IPlugin"/>
        /// </summary>
        /// <param name="pluginFile"></param>
        /// <param name="sharedTypes"></param>
        public void LoadPlugin(string pluginFile, Type[] sharedTypes)
        {
            try
            {
                var loader = PluginLoader.CreateFromAssemblyFile(pluginFile,
                    // this ensures that the plugin resolves to the same version of DependencyInjection
                    // and ASP.NET Core that the current app uses
                    sharedTypes: sharedTypes);
                foreach (var type in loader.LoadDefaultAssembly()
                    .GetTypes()
                    .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    if (typeof(IMiddlewarePlugin).IsAssignableFrom(type))
                    {
                        IMiddlewarePlugin? plugin = (IMiddlewarePlugin) Activator.CreateInstance(type)!;
                        AddMiddlewarePlugin(plugin.EventCode!, plugin);
                    }
                    else
                    {
                        IPlugin? plugin = (IPlugin) Activator.CreateInstance(type)!;
                        AddPlugin(plugin);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error has occurs while loading plugins", ex);
            }
        }
        
        /// <summary>
        /// Load plugins from a given directory and shared types with him
        /// </summary>
        /// <param name="pluginsDirectory"></param>
        /// <param name="sharedTypes"></param>
        public void LoadPlugins(string pluginsDirectory, Type[] sharedTypes)
        {
            try
            {
                foreach (var pluginDir in Directory.GetDirectories(pluginsDirectory))
                {
                    var dirName = Path.GetFileName(pluginDir);
                    var pluginFile = Path.Combine(pluginDir, dirName + ".dll");
                    LoadPlugin(pluginFile, sharedTypes);
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("An error has occurs while loading plugin", e);
            }
        }

        /// <summary>
        /// Configure plugins Services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        private IServiceCollection ConfigurePlugins(IServiceCollection services)
        {
            try
            {
                foreach (var plugin in Plugins.Where(p => p.Enable))
                {
                    plugin.ConfigureServices(services);
                }

                foreach (var plugin in MiddlewarePlugins.SelectMany(middlewarePlugin =>
                    middlewarePlugin.Value.Where(p => p.Enable)))
                {
                    plugin.ConfigureServices(services);
                }

                return services;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Plugins could not be configured, look at the innerException for more details", ex);
            }
        }

        /// <summary>
        /// configure plugins Application Builder
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public IApplicationBuilder ConfigurePlugins(IApplicationBuilder app)
        {
            try
            {
                foreach (var plugin in Plugins.Where(p => p.Enable))
                {
                    plugin.Configure(app);
                }

                foreach (var plugin in MiddlewarePlugins.SelectMany(middlewarePlugin =>
                    middlewarePlugin.Value.Where(p => p.Enable)))
                {
                    plugin.Configure(app);
                }

                return app;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    "Plugins could not be configured, look at the innerException for more details", ex);
            }
        }

        /// <summary>
        /// add a new plugin into the service. if the position is not provided it will
        /// place the plugin at the end of the chain
        /// </summary>
        /// <param name="plugin"></param>
        public virtual void AddPlugin(IPlugin plugin)
        {
            _plugins.Add(plugin);
        }

        /// <summary>
        /// add a new plugin into the service. if the position is not provided it will
        /// place the plugin at the end of the chain. 
        /// </summary>
        /// <param name="eventCode"></param>
        /// <param name="plugin"></param>
        public virtual void AddMiddlewarePlugin(string eventCode, IMiddlewarePlugin plugin)
        {
            if (string.IsNullOrWhiteSpace(eventCode))
            {
                _noEventMiddlewares.Add(plugin);
            }
            else
            {
                if (!_middlewares.ContainsKey(eventCode))
                {
                    _middlewares.Add(eventCode, new List<IMiddlewarePlugin>
                    {
                        plugin
                    });
                }
                else
                {
                    _middlewares[eventCode].Add(plugin);
                }
            }
        }


        /// <summary>
        /// returns a given plugin located within his <see cref="IPlugin.PluginId"/>. It
        /// will search in the plugins first and after in the middleware list.
        /// </summary>
        /// <param name="guid"></param>
        /// <typeparam name="TPlugin"></typeparam>
        /// <returns></returns>
        public virtual Task<TPlugin> Get<TPlugin>(Guid guid)
        {
            var result = _plugins.Find(p => p.PluginId == guid);
            foreach (var tmp in
                _middlewares.Select(middleware => middleware.Value.First(p => p.PluginId == guid))
                    .Where(tmp => tmp != null))
            {
                return Task.FromResult((TPlugin) tmp);
            }

            var tmPlugin = _plugins.First(p => p.PluginId == guid);
            return tmPlugin != null ? Task.FromResult((TPlugin) tmPlugin) : null;
        }
        
        /// <summary>
        /// returns a given plugin located within his <see cref="IPlugin.ShortName"/>. It
        /// will search in the plugins first and after in the middleware list.
        /// </summary>
        /// <param name="shortName"></param>
        /// <typeparam name="TPlugin"></typeparam>
        /// <returns></returns>
        public virtual Task<TPlugin> Get<TPlugin>(string shortName)
        {
            var result = _plugins.Find(p => p.ShortName == shortName);
            foreach (var tmp in
                _middlewares.Select(middleware => middleware.Value.First(p => p.ShortName == shortName))
                    .Where(tmp => tmp != null))
            {
                return Task.FromResult((TPlugin) tmp);
            }

            var tmPlugin = _plugins.First(p => p.ShortName == shortName);
            return tmPlugin != null ? Task.FromResult((TPlugin) tmPlugin) : null;
        }

        /// <summary>
        /// returns the vent middleware chain for the requested event
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        public async Task<ICollection<IMiddlewarePlugin>> MiddlewareChain(string eventCode)
        {
            return await Task.FromResult(_middlewares[eventCode].Where(m => m.Enable).ToList());
        }
    }
}
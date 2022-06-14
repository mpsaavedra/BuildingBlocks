using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Orun.Services;

namespace Orun.Extensions
{
    /// <summary>
    /// <see cref="PluginService"/> extensions
    /// </summary>
    public static class PluginServiceExtensions
    {
        /// <summary>
        /// calls the <see cref="IPluginService">Plugins service</see> and calls
        /// the <see cref="IPluginService.ConfigurePlugins"/> method so plugins could
        /// do initialization or other operations.
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigurePlugins(this IApplicationBuilder app)
        {
            var pluginService = app.ApplicationServices.GetRequiredService<IPluginService>();
            pluginService.ConfigurePlugins(app);
            return app;
        }
        
        /// <summary>
        /// Adds plugins service
        /// </summary>
        /// <param name="services"></param>
        /// <param name="pluginsDirectory"></param>
        /// <param name="sharedTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddPluginsService(this IServiceCollection services, string pluginsDirectory,
            Type[] sharedTypes)
        {
            services.AddSingleton<IPluginService>(new PluginService(
                pluginsDirectory,
                sharedTypes, 
                services));

            services.AddScoped<IChainOfResponsibilityService, ChainOfResponsibilityService>();
            
            return services;
        }

    }
}
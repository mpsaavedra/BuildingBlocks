using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace Orun.Plugins
{
    /// <summary>
    /// Define the plugins basic information. The fields must be defined at
    /// development time to keep it as unique as possible.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Plugin name to show in the plugin menu
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// A plugins short name to identify plugin in a more human readable format
        /// </summary>
        string ShortName { get; }
        
        /// <summary>
        /// Plugin version, if not specified it will be extracted from the assembly version
        /// </summary>
        string Version { get; }
        
        /// <summary>
        /// Plugin unique identifier
        /// </summary>
        Guid PluginId { get; }
        
        /// <summary>
        /// plugin author
        /// </summary>
        string? Author { get; }
        
        /// <summary>
        /// plugin Description
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// List of <see cref="Dependency"/>
        /// </summary>
        ICollection<Dependency>? Dependencies { get; }

        /// <summary>
        /// if true the plugin is enable to be use
        /// </summary>
        bool Enable { get; set; }
    
        /// <summary>
        /// Configure plugin IApplicationBuilder if required. This method could be used to call initialization
        /// processes.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IApplicationBuilder? Configure(IApplicationBuilder builder);
        
        /// <summary>
        /// Configure plugin services if required. This method could be used to call initialization
        /// processes.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        IServiceCollection? ConfigureServices(IServiceCollection services);
    }
}
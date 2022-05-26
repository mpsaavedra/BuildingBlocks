namespace Orun.Plugins
{
    /// <summary>
    /// specify that the plugin is a middleware plugin. You must notice that plugin
    /// execution are order-sensitive, which means that according to the order in
    /// which they are executed and the middleware type (Responsibility Chain or
    /// Pipeline) they could differently affect the result. 
    /// </summary>
    public interface IMiddlewarePlugin : IPlugin
    {
        /// <summary>
        /// Event code, this code is mainly set as a constant name in the
        /// function in which the plugin will be used. Mostly event codes
        /// are refer and used by operations. But if no event-code is specify
        /// it means that current middleware plugin could be used by other
        /// middleware plugins
        /// </summary>
        public string? EventCode { get; }
    }
}
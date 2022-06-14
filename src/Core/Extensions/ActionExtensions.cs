using System;

namespace Orun.Extensions
{
    /// <summary>
    /// <see cref="Action{T}"/> extensions
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// Combine custom action values in <paramref name="action"/> with default <paramref name="action"/>
        /// options 
        /// </summary>
        /// <typeparam name="TOptions">Options class</typeparam>
        /// <param name="action">custom options.</param>
        /// <param name="overrideDefaultOptions">options to overrider with.</param>
        /// <returns>TOptions | ApplicationException: if action is null.</returns>
        public static TOptions ConfigureOrDefault<TOptions>(this Action<TOptions>? action,
            TOptions overrideDefaultOptions = null!)
            where TOptions : class, new()
        {
            overrideDefaultOptions ??= new TOptions();

            action?.Invoke(overrideDefaultOptions);

            return overrideDefaultOptions;
        }
    }
}
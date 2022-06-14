using Microsoft.EntityFrameworkCore;

namespace Orun.Extensions
{
    /// <summary>
    /// <see cref="DbContext"/> extensions
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// if false it won't use LazyLoading or QueryTracking
        /// </summary>
        /// <param name="context">this DbContext</param>
        /// <param name="useTracker">if true it will use the Query change tracking mechanism</param>
        /// <returns></returns>
        public static DbContext UseChangeTracker(this DbContext context, bool useTracker = true)
        {            
            if (!useTracker)
            {
                context.ChangeTracker.LazyLoadingEnabled = false;
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                return context;
            }
            
            // optimize query result
            context.ChangeTracker.LazyLoadingEnabled = true;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

            return context;
        }
    }
}
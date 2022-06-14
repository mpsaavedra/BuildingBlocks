using System.Collections.Generic;
using System.Linq;

namespace Orun.Extensions
{
    /// <summary>
    /// <see cref="IEnumerable{T}"/> extensions
    /// </summary>
    public static class EnumerableExtensions
    {

        public static IEnumerable<T> ToOrder<T>(this IEnumerable<T> list, string sortBy, 
            bool ascending, int offset, int take)
        {
            if (!sortBy.Contains('.'))
            {
                if (ascending)
                    return list.OrderBy(item => item?.GetType()
                        .GetProperty(sortBy)?.GetValue(item)).Skip(offset).Take(take);

                return list.OrderByDescending(item => item?.GetType()
                    .GetProperty(sortBy)?.GetValue(item)).Skip(offset).Take(take);
            }

            else
            {
                var by = sortBy.Split('.');

                if (ascending)
                    return list.OrderBy(item =>
                    {
                        var info = item?.GetType().GetProperty(by[0]);
                        return info?.GetType().GetProperty(by[1])?.GetValue(info.GetValue(item));
                    }).Skip(offset).Take(take);

                return list.OrderByDescending(item =>
                {
                    var get1 = item?.GetType().GetProperty(by[0])?.GetValue(item);
                    return get1?.GetType().GetProperty(by[1])?.GetValue(get1);
                }).Skip(offset).Take(take);
            }
        }
    }
}
using CosmosOdyssey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOdyssey.Extensions
{
    public static class CartesianProduct
    {
        public static IEnumerable<IEnumerable<T>> CrossAllProvidersForRoute<T>(this IEnumerable<IEnumerable<T>> source)
        {
            return source.Aggregate(
                (IEnumerable<IEnumerable<T>>)new[] { Enumerable.Empty<T>() },
                (acc, src) => src.SelectMany(x => acc.Select(a => a.Concat(new[] { x }))));
        }
    }
}

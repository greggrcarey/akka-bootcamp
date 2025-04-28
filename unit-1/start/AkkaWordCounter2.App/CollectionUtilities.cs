using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkkaWordCounter2.App;

public static class CollectionUtilities
{
    public static IImmutableDictionary<string, int> MergeWordCounts(IEnumerable<IDictionary<string, int>> counts)
    {
        ImmutableDictionary<string, int>? mergedCounts = counts.Aggregate(ImmutableDictionary<string, int>.Empty,
                (ImmutableDictionary<string, int> acc, IDictionary<string, int> next) =>
                {
                    foreach (var (word, count) in next)
                    {
                        acc = acc.SetItem(word, acc.GetValueOrDefault(word, 0) + count);
                    }
                    return acc;
                });
        return mergedCounts;
    }
}

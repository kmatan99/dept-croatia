using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dept_croatia.Infrastructure.Extensions
{
    public static class ListExtension
    {
        public static List<T> Page<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            return source.Skip(page - 1).Take(pageSize).ToList();
        }

        public static IEnumerable<List<T>> DoBatching<T>(this IEnumerable<T> source, int batchSize)
        {
            int pageNumber = 1;
            while ((pageNumber - 1) * batchSize < source.Count())
            {
                var batch = source.Page(pageNumber, batchSize).ToList();
                yield return batch;

                pageNumber++;
            }
        }
    }
}

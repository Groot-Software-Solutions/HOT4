using System.Linq.Dynamic.Core;

namespace Hot4.Core.Helper
{
    public static class QuerableFilter
    {
        //     public static async Task<PagedList<T>> ToPagedListAsync<T>(
        //this IQueryable<T> source,
        //int page,
        //int pageSize,
        //CancellationToken token = default)
        //     {
        //         var count = await source.CountAsync(token);
        //         // if (count > 0)

        //         var items = await source
        //             .Skip((page - 1) * pageSize)
        //             .Take(pageSize)
        //             .ToListAsync(token);
        //         return new PagedList<T>(items, count, page, pageSize);
        //         // }

        //         //  return new(Enumerable.Empty<T>(), 0, 0, 0);
        //     }

        public static PagedResult<T> GetPagedData<T>(IQueryable<T> query, int page, int pageSize) where T : class
        {
            var result = new PagedResult<T>
            {
                CurrentPage = page,

                PageSize = pageSize,

                RowCount = query.Count()

            };

            var pageCount = (double)result.RowCount / pageSize;

            result.PageCount = (int)Math.Ceiling(pageCount);

            var skip = (page - 1) * pageSize;

            result.Queryable = query.Skip(skip).Take(pageSize);

            return result;

        }

    }
}

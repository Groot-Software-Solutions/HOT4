﻿using System.Linq.Dynamic.Core;

namespace Hot4.Core.Helper
{
    public static class PaginationFilter
    {
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

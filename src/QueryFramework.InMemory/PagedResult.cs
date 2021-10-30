using System.Collections.Generic;
using CrossCutting.Data.Abstractions;

namespace QueryFramework.InMemory
{
    internal class PagedResult<TResult> : List<TResult>, IPagedResult<TResult> where TResult : class
    {
        private readonly int _totalRecordCount;
        private readonly int _offset;
        private readonly int _pageSize;

        int IPagedResult<TResult>.TotalRecordCount => _totalRecordCount;
        int IPagedResult<TResult>.Offset => _offset;
        int IPagedResult<TResult>.PageSize => _pageSize;

        internal PagedResult(IEnumerable<TResult> source, int totalRecordCount, int offset, int pageSize)
        {
            AddRange(source);
            _totalRecordCount = totalRecordCount;
            _offset = offset;
            _pageSize = pageSize;
        }
    }
}

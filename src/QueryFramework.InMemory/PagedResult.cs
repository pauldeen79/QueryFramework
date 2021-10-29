using System.Collections.Generic;
using CrossCutting.Data.Abstractions;

namespace QueryFramework.InMemory
{
    internal class PagedResult<TResult> : List<TResult>, IPagedResult<TResult> where TResult : class
    {
        private readonly int _totalRecordCount;

        int IPagedResult<TResult>.TotalRecordCount => _totalRecordCount;

        internal PagedResult(IEnumerable<TResult> source, int totalRecordCount)
        {
            AddRange(source);
            _totalRecordCount = totalRecordCount;
        }
    }
}

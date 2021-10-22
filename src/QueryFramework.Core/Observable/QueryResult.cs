using System.Collections.Generic;
using System.Collections.ObjectModel;
using QueryFramework.Abstractions;

namespace QueryFramework.Core.Observable
{
    public class QueryResult<T> : ObservableCollection<T>, IQueryResult<T>
    {
        public int TotalRecordCount { get; }

        public QueryResult(IEnumerable<T> records, int totalRecordCount) : base(records)
        {
            TotalRecordCount = totalRecordCount;
        }
    }
}

using System.Collections.Generic;

namespace QueryFramework.Abstractions
{
    public interface IQueryResult<out T> : IReadOnlyCollection<T>
    {
        int TotalRecordCount { get; }
    }
}

using System.Collections.Generic;
using CrossCutting.Data.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Abstractions
{
    public interface IQueryProcessor<in TQuery, out TResult>
        where TQuery : ISingleEntityQuery
        where TResult : class
    {
        TResult? FindOne(TQuery query);
        IReadOnlyCollection<TResult> FindMany(TQuery query);
        IPagedResult<TResult> FindPaged(TQuery query);
    }

    public interface IQueryProcessor<out TResult> : IQueryProcessor<ISingleEntityQuery, TResult>
        where TResult : class
    {
    }
}

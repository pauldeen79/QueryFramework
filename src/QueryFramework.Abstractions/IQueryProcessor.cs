using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Abstractions
{
    public interface IQueryProcessor<in TQuery, out TResult>
    {
        IQueryResult<TResult> Query(TQuery query);
    }

    public interface IQueryProcessor<out TResult> : IQueryProcessor<ISingleEntityQuery, TResult>
    {
    }
}

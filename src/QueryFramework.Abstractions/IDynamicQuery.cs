using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Abstractions
{
    public interface IDynamicQuery<out TQuery> : ISingleEntityQuery
        where TQuery : ISingleEntityQuery
    {
        TQuery Process();
    }
}

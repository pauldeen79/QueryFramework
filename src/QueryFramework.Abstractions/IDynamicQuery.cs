using QueryFramework.Abstractions.Queries;

namespace QueryFramework.Abstractions
{
    public interface IDynamicQuery : ISingleEntityQuery
    {
        ISingleEntityQuery Process();
    }
}

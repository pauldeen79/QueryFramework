using CrossCutting.Data.Abstractions;
using QueryFramework.Abstractions.Queries;

namespace QueryFramework.SqlServer.Abstractions
{
    public interface IDatabaseCommandGenerator
    {
        IDatabaseCommand Generate<TQuery>(TQuery query,
                                          IQueryProcessorSettings settings,
                                          bool countOnly)
            where TQuery : ISingleEntityQuery;
    }
}

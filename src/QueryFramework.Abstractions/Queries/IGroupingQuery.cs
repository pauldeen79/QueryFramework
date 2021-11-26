using CrossCutting.Common;

namespace QueryFramework.Abstractions.Queries
{
    public interface IGroupingQuery : ISingleEntityQuery
    {
        ValueCollection<IQueryExpression> GroupByFields { get; }
        ValueCollection<IQueryCondition> HavingFields { get; }
    }
}

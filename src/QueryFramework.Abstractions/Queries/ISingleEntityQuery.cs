namespace QueryFramework.Abstractions.Queries;

public interface ISingleEntityQuery
{
    int? Limit { get; }
    int? Offset { get; }
    ValueCollection<ICondition> Conditions { get; }
    ValueCollection<IQuerySortOrder> OrderByFields { get; }
}

namespace QueryFramework.Abstractions.Queries;

public interface ISingleEntityQuery
{
    int? Limit { get; }
    int? Offset { get; }
    IReadOnlyCollection<ICondition> Conditions { get; }
    IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}

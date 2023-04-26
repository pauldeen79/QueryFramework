namespace QueryFramework.Abstractions.Queries;

public interface ISingleEntityQuery
{
    int? Limit { get; }
    int? Offset { get; }
    ComposedEvaluatable Filter { get; }
    IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}

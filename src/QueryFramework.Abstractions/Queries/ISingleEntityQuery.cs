namespace QueryFramework.Abstractions.Queries;

public interface ISingleEntityQuery
{
    int? Limit { get; }
    int? Offset { get; }
    ValueCollection<IQueryCondition> Conditions { get; }
    ValueCollection<IQuerySortOrder> OrderByFields { get; }
}

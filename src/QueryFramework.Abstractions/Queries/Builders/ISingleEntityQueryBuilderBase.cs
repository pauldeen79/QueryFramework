namespace QueryFramework.Abstractions.Queries.Builders;

public interface ISingleEntityQueryBuilderBase
{
    int? Limit { get; set; }
    int? Offset { get; set; }
    ComposedEvaluatableBuilder Filter { get; set; }
    List<IQuerySortOrderBuilder> OrderByFields { get; set; }
}

namespace QueryFramework.Abstractions.Queries.Builders;

public interface IGroupingQueryBuilder : ISingleEntityQueryBuilderBase
{
    List<ExpressionBuilder> GroupByFields { get; set; }
    ComposedEvaluatableBuilder GroupByFilter { get; set; }
    IGroupingQuery Build();
}

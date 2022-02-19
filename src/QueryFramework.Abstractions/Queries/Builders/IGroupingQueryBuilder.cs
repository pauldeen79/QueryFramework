namespace QueryFramework.Abstractions.Queries.Builders;

public interface IGroupingQueryBuilder : ISingleEntityQueryBuilderBase
{
    List<IExpressionBuilder> GroupByFields { get; set; }
    List<IConditionBuilder> HavingFields { get; set; }
    IGroupingQuery Build();
}

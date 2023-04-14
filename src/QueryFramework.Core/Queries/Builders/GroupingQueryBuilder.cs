namespace QueryFramework.Core.Queries.Builders;

public sealed class GroupingQueryBuilder : IGroupingQueryBuilder
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public ComposedEvaluatableBuilder Filter { get; set; }
    public List<IQuerySortOrderBuilder> OrderByFields { get; set; }
    public List<ExpressionBuilder> GroupByFields { get; set; }
    public ComposedEvaluatableBuilder GroupByFilter { get; set; }

    public GroupingQueryBuilder()
    {
        Filter = new();
        OrderByFields = new();
        GroupByFields = new();
        GroupByFilter = new();
    }

    public IGroupingQuery Build()
        => new GroupingQuery(Limit,
                             Offset,
                             Filter.BuildTyped(),
                             GroupByFilter.BuildTyped(),
                             OrderByFields.Select(x => x.Build()),
                             GroupByFields.Select(x => x.Build()));
}

namespace QueryFramework.Core.Queries;

public record GroupingQuery : SingleEntityQuery, IGroupingQuery
{
    public GroupingQuery(
        int? limit,
        int? offset,
        ComposedEvaluatable filter,
        ComposedEvaluatable groupByFilter,
        IEnumerable<IQuerySortOrder> orderByFields,
        IEnumerable<Expression> groupByFields)
        : base(limit, offset, filter, orderByFields)
    {
        GroupByFields = new ReadOnlyValueCollection<Expression>(groupByFields);
        GroupByFilter = groupByFilter;
    }

    protected GroupingQuery(
        SingleEntityQuery original,
        ComposedEvaluatable groupByFilter,
        IEnumerable<Expression> groupByFields)
        : base(original)
    {
        GroupByFields = new ReadOnlyValueCollection<Expression>(groupByFields);
        GroupByFilter = groupByFilter;
    }

    public IReadOnlyCollection<Expression> GroupByFields { get; }

    public ComposedEvaluatable GroupByFilter { get; }
}

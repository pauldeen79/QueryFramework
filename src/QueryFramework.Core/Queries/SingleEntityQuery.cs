namespace QueryFramework.Core.Queries;

public record SingleEntityQuery : ISingleEntityQuery
{
    public SingleEntityQuery() : this(null,
                                      null,
                                      new ComposedEvaluatable(Enumerable.Empty<ComposableEvaluatable>()),
                                      Enumerable.Empty<IQuerySortOrder>())
    {
    }

    public SingleEntityQuery(int? limit,
                             int? offset,
                             ComposedEvaluatable filter,
                             IEnumerable<IQuerySortOrder> orderByFields)
    {
        Limit = limit;
        Offset = offset;
        Filter = filter;
        OrderByFields = new ReadOnlyValueCollection<IQuerySortOrder>(orderByFields);
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public int? Limit { get; }
    public int? Offset { get; }
    public ComposedEvaluatable Filter { get; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}

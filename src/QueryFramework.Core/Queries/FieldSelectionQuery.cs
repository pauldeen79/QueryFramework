namespace QueryFramework.Core.Queries;

public record FieldSelectionQuery : IFieldSelectionQuery
{
    public FieldSelectionQuery() : this(null,
                                        null,
                                        false,
                                        false,
                                        new ComposedEvaluatable(Enumerable.Empty<ComposableEvaluatable>()),
                                        Enumerable.Empty<IQuerySortOrder>(),
                                        Enumerable.Empty<Expression>())
    {
    }

    public FieldSelectionQuery(int? limit,
                               int? offset,
                               bool distinct,
                               bool getAllFields,
                               ComposedEvaluatable filter,
                               IEnumerable<IQuerySortOrder> orderByFields,
                               IEnumerable<Expression> fields)
    {
        Limit = limit;
        Offset = offset;
        Distinct = distinct;
        GetAllFields = getAllFields;
        Fields = new ReadOnlyValueCollection<Expression>(fields);
        Filter = filter;
        OrderByFields = new ReadOnlyValueCollection<IQuerySortOrder>(orderByFields);
        Validator.ValidateObject(this, new ValidationContext(this, null, null), true);
    }

    public int? Limit { get; }
    public int? Offset { get; }
    public bool Distinct { get; }
    public bool GetAllFields { get; }
    public IReadOnlyCollection<Expression> Fields { get; }
    public ComposedEvaluatable Filter { get; }
    public IReadOnlyCollection<IQuerySortOrder> OrderByFields { get; }
}

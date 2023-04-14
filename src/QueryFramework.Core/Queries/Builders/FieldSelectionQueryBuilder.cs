namespace QueryFramework.Core.Queries.Builders;

public sealed class FieldSelectionQueryBuilder : IFieldSelectionQueryBuilder
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public bool Distinct { get; set; }
    public bool GetAllFields { get; set; }
    public List<string> FieldNames { get; set; }
    public ComposedEvaluatableBuilder Filter { get; set; }
    public List<IQuerySortOrderBuilder> OrderByFields { get; set; }

    public FieldSelectionQueryBuilder()
    {
        FieldNames = new();
        Filter = new();
        OrderByFields = new();
    }

    public IFieldSelectionQuery Build()
        => new FieldSelectionQuery(Limit,
                                   Offset,
                                   Distinct,
                                   GetAllFields,
                                   Filter.BuildTyped(),
                                   OrderByFields.Select(x => x.Build()),
                                   FieldNames);
}

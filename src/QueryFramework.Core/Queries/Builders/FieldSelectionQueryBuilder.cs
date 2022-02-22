namespace QueryFramework.Core.Queries.Builders;

public sealed class FieldSelectionQueryBuilder : IFieldSelectionQueryBuilder
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
    public bool Distinct { get; set; }
    public bool GetAllFields { get; set; }
    public List<IExpressionBuilder> Fields { get; set; }
    public List<IConditionBuilder> Conditions { get; set; }
    public List<IQuerySortOrderBuilder> OrderByFields { get; set; }

    public FieldSelectionQueryBuilder()
    {
        Fields = new List<IExpressionBuilder>();
        Conditions = new List<IConditionBuilder>();
        OrderByFields = new List<IQuerySortOrderBuilder>();
    }

    public IFieldSelectionQuery Build()
        => new FieldSelectionQuery(Limit,
                                   Offset,
                                   Distinct,
                                   GetAllFields,
                                   Conditions.Select(x => x.Build()),
                                   OrderByFields.Select(x => x.Build()),
                                   Fields.Select(x => x.Build()));
}

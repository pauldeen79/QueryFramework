namespace QueryFramework.Abstractions.Queries.Builders;

public interface IFieldSelectionQueryBuilder : ISingleEntityQueryBuilderBase
{
    bool Distinct { get; set; }
    bool GetAllFields { get; set; }
    List<string> FieldNames { get; set; }
    IFieldSelectionQuery Build();
}

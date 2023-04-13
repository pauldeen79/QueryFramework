namespace QueryFramework.Abstractions.Queries.Builders;

public interface IFieldSelectionQueryBuilder : ISingleEntityQueryBuilderBase
{
    bool Distinct { get; set; }
    bool GetAllFields { get; set; }
    List<ExpressionBuilder> Fields { get; set; }
    IFieldSelectionQuery Build();
}

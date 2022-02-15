namespace ExpressionFramework.Core.DomainModel.Builders;

public partial class FieldExpressionBuilder
{
    public FieldExpressionBuilder WithFieldName(string fieldName)
        => this.Chain(x => x.FieldName = fieldName);
}

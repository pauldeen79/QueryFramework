namespace ExpressionFramework.Core.DomainModel.Builders;

public partial class ConstantExpressionBuilder
{
    public ConstantExpressionBuilder WithValue(object? value)
        => this.Chain(x => x.Value = value);
}

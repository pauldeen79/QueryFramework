namespace ExpressionFramework.Core.DomainModel.Builders;

public partial class DelegateExpressionBuilder
{
    public DelegateExpressionBuilder WithValue(Func<object?> valueDelegate)
        => this.Chain(x => x.ValueDelegate = valueDelegate);
}

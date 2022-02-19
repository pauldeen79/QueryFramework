namespace ExpressionFramework.Core.Functions;

public class SumFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionBuilder Expression { get; set; } = new EmptyExpressionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public SumFunctionBuilder WithExpression(IExpressionBuilder expression)
        => this.Chain(x => x.Expression = expression);

    public IExpressionFunction Build()
        => new SumFunction(Expression.Build(), InnerFunction?.Build());
}

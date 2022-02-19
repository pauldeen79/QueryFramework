namespace ExpressionFramework.Core.Functions;

public class YearFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionBuilder Expression { get; set; } = new EmptyExpressionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public YearFunctionBuilder WithExpression(IExpressionBuilder expression)
        => this.Chain(x => x.Expression = expression);

    public IExpressionFunction Build()
        => new YearFunction(Expression.Build(), InnerFunction?.Build());
}

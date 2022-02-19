namespace ExpressionFramework.Core.Functions;

public class MonthFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionBuilder Expression { get; set; } = new EmptyExpressionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public MonthFunctionBuilder WithExpression(IExpressionBuilder expression)
        => this.Chain(x => x.Expression = expression);

    public IExpressionFunction Build()
        => new MonthFunction(Expression.Build(), InnerFunction?.Build());
}

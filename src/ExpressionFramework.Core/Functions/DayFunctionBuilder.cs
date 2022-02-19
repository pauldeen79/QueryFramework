namespace ExpressionFramework.Core.Functions;

public class DayFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionBuilder Expression { get; set; } = new EmptyExpressionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public DayFunctionBuilder WithExpression(IExpressionBuilder expression)
        => this.Chain(x => x.Expression = expression);

    public IExpressionFunction Build()
        => new DayFunction(Expression.Build(), InnerFunction?.Build());
}

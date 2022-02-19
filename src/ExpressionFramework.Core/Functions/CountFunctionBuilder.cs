namespace ExpressionFramework.Core.Functions;

public class CountFunctionBuilder : IExpressionFunctionBuilder
{
    public IExpressionBuilder Expression { get; set; } = new EmptyExpressionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public CountFunctionBuilder WithExpression(IExpressionBuilder expression)
        => this.Chain(x => x.Expression = expression);

    public IExpressionFunction Build()
        => new CountFunction(Expression.Build(), InnerFunction?.Build());
}

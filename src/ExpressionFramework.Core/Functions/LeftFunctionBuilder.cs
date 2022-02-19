namespace ExpressionFramework.Core.Functions;

public class LeftFunctionBuilder : IExpressionFunctionBuilder
{
    public int Length { get; set; }
    public IExpressionBuilder Expression { get; set; } = new EmptyExpressionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public LeftFunctionBuilder WithExpression(IExpressionBuilder expression)
        => this.Chain(x => x.Expression = expression);

    public LeftFunctionBuilder WithLength(int length)
        => this.Chain(x => x.Length = length);

    public IExpressionFunction Build()
        => new LeftFunction(Length, Expression.Build(), InnerFunction?.Build());
}

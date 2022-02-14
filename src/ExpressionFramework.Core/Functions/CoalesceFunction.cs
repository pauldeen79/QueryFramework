namespace ExpressionFramework.Core.Functions;

public record CoalesceFunction : IExpression, IExpressionFunction
{
    public CoalesceFunction(IExpressionFunction? innerFunction,
                            params IExpression[] innerExpressions)
    {
        InnerFunction = innerFunction;
        InnerExpressions = innerExpressions;
    }

    public CoalesceFunction(IExpressionFunction? innerFunction,
                            IEnumerable<IExpression> innerExpressions)
        : this(innerFunction, innerExpressions.ToArray())
    {
    }

    public IExpressionFunction? InnerFunction { get; }

    public IEnumerable<IExpression> InnerExpressions { get; }

    public IExpressionFunction? Function => this;

    public IExpressionFunctionBuilder ToBuilder()
        => new CoalesceFunctionBuilder
        {
            InnerFunction = InnerFunction?.ToBuilder(),
            InnerExpressions = InnerExpressions.Select(x => new ExpressionBuilder(x)).Cast<IExpressionBuilder>().ToList()
        };
}

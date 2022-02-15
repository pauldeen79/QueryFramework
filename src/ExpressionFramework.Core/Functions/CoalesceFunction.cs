namespace ExpressionFramework.Core.Functions;

public record CoalesceFunction : IExpression, IExpressionFunction
{
    public CoalesceFunction(IExpressionFunction? innerFunction,
                            IEnumerable<IExpression> innerExpressions)
    {
        InnerFunction = innerFunction;
        InnerExpressions = innerExpressions;
    }

    public IExpressionFunction? InnerFunction { get; }

    public IEnumerable<IExpression> InnerExpressions { get; }

    public IExpressionFunction? Function => this;

    private CoalesceFunctionBuilder CreateBuilder()
        => new CoalesceFunctionBuilder().WithInnerFunction(InnerFunction?.ToBuilder())
                                        .AddInnerExpressions(InnerExpressions.Select(x => x.ToBuilder()));

    IExpressionBuilder IExpression.ToBuilder() => CreateBuilder();

    IExpressionFunctionBuilder IExpressionFunction.ToBuilder() => CreateBuilder();
}

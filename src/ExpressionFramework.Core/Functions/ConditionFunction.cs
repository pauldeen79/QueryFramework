namespace ExpressionFramework.Core.Functions;

public record class ConditionFunction : IExpressionFunction
{
    public ICondition Condition { get; }
    public IExpressionFunction? InnerFunction { get; }

    public ConditionFunction(ICondition condition, IExpressionFunction? innerFunction)
    {
        Condition = condition;
        InnerFunction = innerFunction;
    }

    public IExpressionFunctionBuilder ToBuilder()
        => new ConditionFunctionBuilder().WithCondition(new ConditionBuilder(Condition))
                                         .WithInnerFunction(InnerFunction?.ToBuilder());
}

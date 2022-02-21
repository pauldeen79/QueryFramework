namespace ExpressionFramework.Core.Functions;

public record class ConditionFunction : IExpressionFunction
{
    public ValueCollection<ICondition> Conditions { get; }
    public IExpressionFunction? InnerFunction { get; }

    public ConditionFunction(IEnumerable<ICondition> conditions, IExpressionFunction? innerFunction)
    {
        Conditions = new ValueCollection<ICondition>(conditions);
        InnerFunction = innerFunction;
    }

    public IExpressionFunctionBuilder ToBuilder()
        => new ConditionFunctionBuilder().AddConditions(Conditions.Select(x => new ConditionBuilder(x)))
                                         .WithInnerFunction(InnerFunction?.ToBuilder());
}

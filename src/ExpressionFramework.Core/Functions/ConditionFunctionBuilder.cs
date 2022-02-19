namespace ExpressionFramework.Core.Functions;

public class ConditionFunctionBuilder : IExpressionFunctionBuilder
{
    public IConditionBuilder Condition { get; set; } = new ConditionBuilder();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public ConditionFunctionBuilder WithCondition(IConditionBuilder condition)
        => this.Chain(x => x.Condition = condition);

    public IExpressionFunction Build()
        => new ConditionFunction(Condition.Build(), InnerFunction?.Build());
}

namespace ExpressionFramework.Core.Functions;

public class ConditionFunctionBuilder : IExpressionFunctionBuilder
{
    public List<IConditionBuilder> Conditions { get; set; } = new List<IConditionBuilder>();
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public ConditionFunctionBuilder AddConditions(params IConditionBuilder[] conditions)
        => this.Chain(x => x.Conditions.AddRange(conditions));

    public ConditionFunctionBuilder AddConditions(IEnumerable<IConditionBuilder> conditions)
        => this.Chain(x => x.Conditions.AddRange(conditions));

    public IExpressionFunction Build()
        => new ConditionFunction(Conditions.Select(x => x.Build()), InnerFunction?.Build());
}

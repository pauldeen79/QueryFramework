namespace ExpressionFramework.Core.Functions;

public class CoalesceFunctionBuilder : IExpressionBuilder, IExpressionFunctionBuilder
{
    public IExpressionFunctionBuilder? Function { get; set; }
    public List<IExpressionBuilder> InnerExpressions { get; set; }
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpression Build()
        => new CoalesceFunction(Function?.Build(), InnerExpressions.Select(x => x.Build()));

    public CoalesceFunctionBuilder()
        => InnerExpressions = new List<IExpressionBuilder>();

    public CoalesceFunctionBuilder WithFunction(IExpressionFunctionBuilder? function)
        => this.Chain(x => x.Function = function);

    public CoalesceFunctionBuilder AddInnerExpressions(params IExpressionBuilder[] innerExpressions)
        => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

    public CoalesceFunctionBuilder AddInnerExpressions(IEnumerable<IExpressionBuilder> innerExpressions)
        => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

    IExpressionFunction IExpressionFunctionBuilder.Build()
        => new CoalesceFunction(Function?.Build(), InnerExpressions.Select(x => x.Build()));
}

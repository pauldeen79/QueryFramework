namespace ExpressionFramework.Core.Functions;

public class CoalesceFunctionBuilder : IExpressionBuilder, IExpressionFunctionBuilder
{
    public string FieldName { get; set; }
    public IExpressionFunctionBuilder? Function { get; set; }
    public List<IExpressionBuilder> InnerExpressions { get; set; }
    public IExpressionFunctionBuilder? InnerFunction { get; set; }

    public IExpression Build()
        => new CoalesceFunction(FieldName, Function?.Build(), InnerExpressions.Select(x => x.Build()));

    public CoalesceFunctionBuilder()
    {
        FieldName = string.Empty;
        InnerExpressions = new List<IExpressionBuilder>();
    }

    public CoalesceFunctionBuilder WithFieldName(string fieldName)
        => this.Chain(x => x.FieldName = fieldName);

    public CoalesceFunctionBuilder WithFunction(IExpressionFunctionBuilder? function)
        => this.Chain(x => x.Function = function);

    public CoalesceFunctionBuilder AddInnerExpressions(params IExpressionBuilder[] innerExpressions)
        => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

    public CoalesceFunctionBuilder AddInnerExpressions(IEnumerable<IExpressionBuilder> innerExpressions)
        => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

    IExpressionFunction IExpressionFunctionBuilder.Build()
        => new CoalesceFunction(FieldName, Function?.Build(), InnerExpressions.Select(x => x.Build()));
}

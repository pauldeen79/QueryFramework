namespace QueryFramework.Core.Functions;

public class CoalesceFunctionBuilder : IQueryExpressionBuilder, IQueryExpressionFunctionBuilder
{
    public string FieldName { get; set; }
    public IQueryExpressionFunctionBuilder? Function { get; set; }
    public List<IQueryExpressionBuilder> InnerExpressions { get; set; }
    public IQueryExpressionFunctionBuilder? InnerFunction { get; set; }

    public IQueryExpression Build()
        => new CoalesceFunction(FieldName, Function?.Build(), InnerExpressions.Select(x => x.Build()));

    public CoalesceFunctionBuilder()
    {
        FieldName = string.Empty;
        InnerExpressions = new List<IQueryExpressionBuilder>();
    }

    public CoalesceFunctionBuilder WithFieldName(string fieldName)
        => this.Chain(x => x.FieldName = fieldName);

    public CoalesceFunctionBuilder WithFunction(IQueryExpressionFunctionBuilder? function)
        => this.Chain(x => x.Function = function);

    public CoalesceFunctionBuilder AddInnerExpressions(params IQueryExpressionBuilder[] innerExpressions)
        => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

    public CoalesceFunctionBuilder AddInnerExpressions(IEnumerable<IQueryExpressionBuilder> innerExpressions)
        => this.Chain(x => x.InnerExpressions.AddRange(innerExpressions));

    IQueryExpressionFunction IQueryExpressionFunctionBuilder.Build()
        => new CoalesceFunction(FieldName, Function?.Build(), InnerExpressions.Select(x => x.Build()));
}

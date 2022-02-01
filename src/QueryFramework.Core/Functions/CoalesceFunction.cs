namespace QueryFramework.Core.Functions;

public record CoalesceFunction : IQueryExpression, IQueryExpressionFunction
{
    public CoalesceFunction(string fieldName,
                            IQueryExpressionFunction? innerFunction,
                            params IQueryExpression[] innerExpressions)
    {
        if (!innerExpressions.Any() && string.IsNullOrEmpty(fieldName))
        {
            throw new ArgumentException("There must be at least one inner expression", nameof(innerExpressions));
        }
        FieldName = fieldName;
        InnerFunction = innerFunction;
        InnerExpressions = innerExpressions;
    }

    public CoalesceFunction(string fieldName,
                            IQueryExpressionFunction? innerFunction,
                            IEnumerable<IQueryExpression> innerExpressions)
        : this(fieldName, innerFunction, innerExpressions.ToArray())
    {
    }

    public IQueryExpressionFunction? InnerFunction { get; }

    public IEnumerable<IQueryExpression> InnerExpressions { get; }

    public string FieldName { get; }

    public IQueryExpressionFunction? Function => this;

    public IQueryExpressionFunctionBuilder ToBuilder()
        => new CoalesceFunctionBuilder
        {
            FieldName = FieldName,
            InnerFunction = InnerFunction?.ToBuilder(),
            InnerExpressions = InnerExpressions.Select(x => new QueryExpressionBuilder(x)).Cast<IQueryExpressionBuilder>().ToList()
        };
}

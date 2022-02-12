namespace ExpressionFramework.Core.Functions;

public record CoalesceFunction : IExpression, IExpressionFunction
{
    public CoalesceFunction(string fieldName,
                            IExpressionFunction? innerFunction,
                            params IExpression[] innerExpressions)
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
                            IExpressionFunction? innerFunction,
                            IEnumerable<IExpression> innerExpressions)
        : this(fieldName, innerFunction, innerExpressions.ToArray())
    {
    }

    public IExpressionFunction? InnerFunction { get; }

    public IEnumerable<IExpression> InnerExpressions { get; }

    public string FieldName { get; }

    public IExpressionFunction? Function => this;

    public IExpressionFunctionBuilder ToBuilder()
        => new CoalesceFunctionBuilder
        {
            FieldName = FieldName,
            InnerFunction = InnerFunction?.ToBuilder(),
            InnerExpressions = InnerExpressions.Select(x => new ExpressionBuilder(x)).Cast<IExpressionBuilder>().ToList()
        };
}

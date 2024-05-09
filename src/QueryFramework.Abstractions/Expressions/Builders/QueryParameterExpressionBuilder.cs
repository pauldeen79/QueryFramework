namespace QueryFramework.Abstractions.Expressions.Builders;

public class QueryParameterExpressionBuilder : ExpressionBuilder
{
    public QueryParameterExpressionBuilder()
    {
        ParameterName = string.Empty;
    }

    public QueryParameterExpressionBuilder(QueryParameterExpression source) : base(source)
    {
        source = source.IsNotNull(nameof(source));

        ParameterName = source.ParameterName;
    }

    [Required]
    public string ParameterName { get; set; }

    public override Expression Build() => new QueryParameterExpression(ParameterName);

    public QueryParameterExpressionBuilder WithParameterName(string parameterName)
    {
        ParameterName = parameterName.IsNotNull(nameof(parameterName));
        return this;
    }
}

namespace QueryFramework.Abstractions.Expressions.Builders;

public class QueryParameterExpressionBuilder : IBuilder<IExpressionComponent>
{
    public QueryParameterExpressionBuilder()
    {
        ParameterName = string.Empty;
    }

    public QueryParameterExpressionBuilder(QueryParameterExpression source)
    {
        source = source.IsNotNull(nameof(source));

        ParameterName = source.ParameterName;
    }

    [Required]
    public string ParameterName { get; set; }

    public QueryParameterExpressionBuilder WithParameterName(string parameterName)
    {
        ParameterName = parameterName.IsNotNull(nameof(parameterName));
        return this;
    }

    public IExpressionComponent Build() => new QueryParameterExpression(ParameterName);
}

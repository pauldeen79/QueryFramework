namespace QueryFramework.SqlServer;

public class DefaultSqlExpressionEvaluator : ISqlExpressionEvaluator
{
    private readonly IEnumerable<ISqlExpressionEvaluatorProvider> _sqlExpressionEvaluatorProviders;
    private readonly IEnumerable<IFunctionParser> _functionParsers;

    public DefaultSqlExpressionEvaluator(IEnumerable<ISqlExpressionEvaluatorProvider> sqlExpressionEvaluatorProviders,
                                         IEnumerable<IFunctionParser> functionParsers)
    {
        _sqlExpressionEvaluatorProviders = sqlExpressionEvaluatorProviders;
        _functionParsers = functionParsers;
    }

    public string GetSqlExpression(IQuery query, Expression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag)
    {
        if (expression is IUntypedExpressionProvider untypedProvider)
        {
            expression = untypedProvider.ToUntyped();
        }

        var result = default(string?);
        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetSqlExpression(query, expression, this, fieldInfo, parameterBag, out var providerResult))
            {
                result = providerResult ?? string.Empty;
                break;
            }
        }

        if (result is null)
        {
            result = TryGetSqlExpression(expression, out var innerExpression);
            if (result is null)
            {
                throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
            }
            var innerResult = GetSqlExpression(query, innerExpression, fieldInfo, parameterBag);
            return result.Replace("{0}", innerResult);
        }

        return result;
    }

    public string GetLengthExpression(IQuery query, Expression expression, IQueryFieldInfo fieldInfo)
    {
        if (expression is IUntypedExpressionProvider untypedProvider)
        {
            expression = untypedProvider.ToUntyped();
        }

        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetLengthExpression(query, expression, this, fieldInfo, out var providerResult))
            {
                return providerResult ?? string.Empty;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
    }

    private string? TryGetSqlExpression(Expression expression, out Expression innerExpression)
    {
        foreach (var parser in _functionParsers)
        {
            if (parser.TryParse(expression, this, out var sqlExpression, out var ex))
            {
                innerExpression = ex;
                return sqlExpression;
            }
        }

        innerExpression = new EmptyExpression();
        return default;
    }
}

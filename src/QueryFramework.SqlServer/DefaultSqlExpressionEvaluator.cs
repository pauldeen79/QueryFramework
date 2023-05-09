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

    public string GetSqlExpression(Expression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context)
    {
        if (expression is IUntypedExpressionProvider untypedProvider)
        {
            expression = untypedProvider.ToUntyped();
        }

        var result = default(string?);
        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetSqlExpression(expression, this, fieldInfo, parameterBag, context, out var providerResult))
            {
                result = providerResult ?? string.Empty;
                break;
            }
        }

        Expression? innerExpression = null;

        if (result is null)
        {
            innerExpression = expression.TryGetInnerExpression();
            if (innerExpression is null)
            {
                throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
            }

            var innerResult = GetSqlExpression(innerExpression, fieldInfo, parameterBag, context);
            result = TryGetSqlExpression(expression)?.Replace("{0}", innerResult) ?? throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
        }

        return result;
    }

    public string GetLengthExpression(Expression expression, IQueryFieldInfo fieldInfo, object? context)
    {
        if (expression is IUntypedExpressionProvider untypedProvider)
        {
            expression = untypedProvider.ToUntyped();
        }

        foreach (var sqlExpressionEvaluatorProvider in _sqlExpressionEvaluatorProviders)
        {
            if (sqlExpressionEvaluatorProvider.TryGetLengthExpression(expression, this, fieldInfo, context, out var providerResult))
            {
                return providerResult ?? string.Empty;
            }
        }

        throw new ArgumentOutOfRangeException(nameof(expression), $"Unsupported expression: [{expression.GetType().Name}]");
    }

    private string? TryGetSqlExpression(Expression expression)
    {
        foreach (var parser in _functionParsers)
        {
            if (parser.TryParse(expression, this, out var sqlExpression))
            {
                return sqlExpression;
            }
        }

        return default;
    }
}

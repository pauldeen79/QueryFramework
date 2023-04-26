namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class EmptyExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, object? context, out string? result)
    {
        if (!(expression is EmptyExpression))
        {
            result = null;
            return false;
        }

        result = "0";
        return true;
    }

    public bool TryGetSqlExpression(Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context, out string? result)
    {
        result = null;
        return expression is EmptyExpression;
    }
}

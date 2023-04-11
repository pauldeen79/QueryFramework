namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class EmptyExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (!(expression is IEmptyExpression))
        {
            result = null;
            return false;
        }

        result = "0";
        return true;
    }

    public bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result)
    {
        result = null;
        return expression is IEmptyExpression;
    }
}

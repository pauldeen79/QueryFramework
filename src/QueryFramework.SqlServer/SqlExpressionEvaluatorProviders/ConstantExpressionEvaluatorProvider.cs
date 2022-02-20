namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class ConstantExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (!(expression is IConstantExpression constantExpression))
        {
            result = null;
            return false;
        }

        result = constantExpression.Value.ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, int paramCounter, out string? result)
    {
        if (!(expression is IConstantExpression constantExpression))
        {
            result = null;
            return false;
        }

        result = GetQueryParameterName(paramCounter, constantExpression.Value);
        return true;
    }

    private static string GetQueryParameterName(int paramCounter, object? value)
    {
        if (value is KeyValuePair<string, object> keyValuePair)
        {
            return $"@{keyValuePair.Key}";
        }

        if (value is IQueryParameterValue queryParameterValue)
        {
            return $"@{queryParameterValue.Name}";
        }

        return $"@p{paramCounter}";
    }
}

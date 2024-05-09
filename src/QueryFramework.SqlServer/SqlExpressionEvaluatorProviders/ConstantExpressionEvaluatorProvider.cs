namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class ConstantExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (expression is not ConstantExpression constantExpression)
        {
            result = null;
            return false;
        }

        result = constantExpression.Value.ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result)
    {
        if (expression is not ConstantExpression constantExpression)
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(constantExpression.Value);
        return true;
    }
}

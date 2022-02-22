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

    public bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result)
    {
        if (!(expression is IConstantExpression constantExpression))
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(constantExpression.Value);
        return true;
    }
}

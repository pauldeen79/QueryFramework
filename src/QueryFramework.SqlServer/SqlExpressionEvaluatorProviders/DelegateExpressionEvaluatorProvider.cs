namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, object? context, out string? result)
    {
        if (!(expression is DelegateExpression delegateExpression))
        {
            result = null;
            return false;
        }

        result = delegateExpression.Value.Invoke(context).ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context, out string? result)
    {
        if (!(expression is DelegateExpression delegateExpression))
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(delegateExpression.Value.Invoke(context));
        return true;
    }
}

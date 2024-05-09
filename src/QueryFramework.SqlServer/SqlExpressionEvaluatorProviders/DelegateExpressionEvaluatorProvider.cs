namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, object? context, out string? result)
    {
        if (expression is not DelegateExpression delegateExpression)
        {
            result = null;
            return false;
        }

        result = delegateExpression.Value.Invoke(context).ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context, out string? result)
    {
        if (expression is not DelegateExpression delegateExpression)
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(delegateExpression.Value.Invoke(context));
        return true;
    }
}

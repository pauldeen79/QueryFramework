namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class ContextExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, object? context, out string? result)
    {
        if (expression is not ContextExpression)
        {
            result = null;
            return false;
        }

        result = context.ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(IQuery query, Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context, out string? result)
    {
        if (expression is not ContextExpression)
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(context);
        return true;
    }
}

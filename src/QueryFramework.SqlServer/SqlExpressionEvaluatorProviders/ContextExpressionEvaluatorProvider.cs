namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class ContextExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    public bool TryGetLengthExpression(Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, object? context, out string? result)
    {
        if (expression is IUntypedExpressionProvider x)
        {
            expression = x.ToUntyped();
        }

        if (!(expression is ContextExpression))
        {
            result = null;
            return false;
        }

        result = context.ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(Expression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, object? context, out string? result)
    {
        if (expression is IUntypedExpressionProvider x)
        {
            expression = x.ToUntyped();
        }

        if (!(expression is ContextExpression))
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(context);
        return true;
    }
}

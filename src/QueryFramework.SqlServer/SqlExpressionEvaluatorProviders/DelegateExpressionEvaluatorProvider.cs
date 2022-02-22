namespace QueryFramework.SqlServer.SqlExpressionEvaluatorProviders;

public class DelegateExpressionEvaluatorProvider : ISqlExpressionEvaluatorProvider
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public DelegateExpressionEvaluatorProvider(IExpressionEvaluator expressionEvaluator)
        => _expressionEvaluator = expressionEvaluator;

    public bool TryGetLengthExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, out string? result)
    {
        if (!(expression is IDelegateExpression delegateExpression))
        {
            result = null;
            return false;
        }

        result = delegateExpression.ValueDelegate.Invoke(null, expression, _expressionEvaluator).ToStringWithNullCheck().Length.ToString();
        return true;
    }

    public bool TryGetSqlExpression(IExpression expression, ISqlExpressionEvaluator evaluator, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, out string? result)
    {
        if (!(expression is IDelegateExpression delegateExpression))
        {
            result = null;
            return false;
        }

        result = parameterBag.CreateQueryParameterName(delegateExpression.ValueDelegate.Invoke(null, expression, _expressionEvaluator));
        return true;
    }
}

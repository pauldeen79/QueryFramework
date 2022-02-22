namespace QueryFramework.SqlServer.FunctionParsers;

public class ConditionFunctionFunctionParser : IFunctionParser
{
    private readonly IExpressionEvaluator _expressionEvaluator;

    public ConditionFunctionFunctionParser(IExpressionEvaluator expressionEvaluator)
        => _expressionEvaluator = expressionEvaluator;

    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is ConditionFunction f)
        {
            // Note that we ignore the return value, because TryEvaluate will always return true in this case. (we're feeding a ConditionFunction)
            _ = new ConditionFunctionEvaluator().TryEvaluate(function, null, _expressionEvaluator, out var result);
            sqlExpression = Convert.ToBoolean(result)
                ? "{0}"
                : "NULL";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

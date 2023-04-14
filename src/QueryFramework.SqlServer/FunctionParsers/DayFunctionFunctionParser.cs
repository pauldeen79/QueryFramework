namespace QueryFramework.SqlServer.FunctionParsers;

public class DayFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is DayExpression)
        {
            sqlExpression = "DAY({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

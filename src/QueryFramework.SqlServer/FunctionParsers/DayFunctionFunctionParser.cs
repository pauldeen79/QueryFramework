namespace QueryFramework.SqlServer.FunctionParsers;

public class DayFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is DayFunction f)
        {
            sqlExpression = "DAY({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

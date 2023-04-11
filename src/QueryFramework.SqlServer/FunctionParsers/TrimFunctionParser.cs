namespace QueryFramework.SqlServer.FunctionParsers;

public class TrimFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is TrimFunction)
        {
            sqlExpression = "TRIM({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

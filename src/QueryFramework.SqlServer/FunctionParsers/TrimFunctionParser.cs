namespace QueryFramework.SqlServer.FunctionParsers;

public class TrimFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is TrimFunction f)
        {
            sqlExpression = "TRIM({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

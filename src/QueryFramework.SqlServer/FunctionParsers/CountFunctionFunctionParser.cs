namespace QueryFramework.SqlServer.FunctionParsers;

public class CountFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is CountFunction)
        {
            sqlExpression = "COUNT({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

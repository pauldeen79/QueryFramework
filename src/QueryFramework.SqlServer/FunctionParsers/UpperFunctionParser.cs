namespace QueryFramework.SqlServer.FunctionParsers;

public class UpperFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is UpperFunction)
        {
            sqlExpression = "UPPER({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

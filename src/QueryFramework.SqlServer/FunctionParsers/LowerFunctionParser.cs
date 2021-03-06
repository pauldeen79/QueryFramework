namespace QueryFramework.SqlServer.FunctionParsers;

public class LowerFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is LowerFunction f)
        {
            sqlExpression = "LOWER({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

namespace QueryFramework.SqlServer.FunctionParsers;

public class SumFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is SumFunction f)
        {
            sqlExpression = "SUM({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

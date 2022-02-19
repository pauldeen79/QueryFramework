namespace QueryFramework.SqlServer.FunctionParsers;

public class SumFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
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

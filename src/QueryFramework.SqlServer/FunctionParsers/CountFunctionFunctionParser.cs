namespace QueryFramework.SqlServer.FunctionParsers;

public class CountFunctionFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is CountFunction f)
        {
            sqlExpression = "COUNT({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

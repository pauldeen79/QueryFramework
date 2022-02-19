namespace QueryFramework.SqlServer.FunctionParsers;

public class YearFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is YearFunction f)
        {
            sqlExpression = "YEAR({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

namespace QueryFramework.SqlServer.FunctionParsers;

public class MonthFunctionParser : IFunctionParser
{
    public bool TryParse(IQueryExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is MonthFunction f)
        {
            sqlExpression = "MONTH({0})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

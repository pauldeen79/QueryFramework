namespace QueryFramework.SqlServer.FunctionParsers;

public class RightFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, IQueryExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is RightFunction f)
        {
            sqlExpression = $"RIGHT({{0}}, {f.Length})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

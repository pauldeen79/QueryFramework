namespace QueryFramework.SqlServer.FunctionParsers;

public class LeftFunctionParser : IFunctionParser
{
    public bool TryParse(IExpressionFunction function, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (function is LeftFunction f)
        {
            sqlExpression = $"LEFT({{0}}, {f.Length})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

namespace QueryFramework.SqlServer.FunctionParsers;

public class LeftFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is LeftExpression f)
        {
            sqlExpression = $"LEFT({{0}}, {f.LengthExpression.EvaluateTyped().Value})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}

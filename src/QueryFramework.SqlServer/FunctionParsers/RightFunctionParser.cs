namespace QueryFramework.SqlServer.FunctionParsers;

public class RightFunctionParser : IFunctionParser
{
    public bool TryParse(Expression expression, ISqlExpressionEvaluator evaluator, out string sqlExpression)
    {
        if (expression is RightExpression f)
        {
            sqlExpression = $"RIGHT({{0}}, {f.LengthExpression.EvaluateTyped().Value})";
            return true;
        }

        sqlExpression = string.Empty;
        return false;
    }
}
